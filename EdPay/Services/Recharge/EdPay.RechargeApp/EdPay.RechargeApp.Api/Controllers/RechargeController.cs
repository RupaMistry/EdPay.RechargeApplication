namespace EdPay.RechargeApp.Api.Controllers
{
    /// <summary>
    /// Recharge API Controller
    /// </summary>
    /// <param name="rechargePaymentService"></param>
    /// <param name="paymentApiClient"></param>
    /// <param name="logger"></param> 
    [ApiController]
    [Route("api/recharge")]
    public class RechargeController(
        IRechargePaymentService<RechargePayment> rechargePaymentService, 
        ILogger<RechargeController> logger, 
        PaymentApiClient paymentApiClient) : ControllerBase
    {       
        private readonly IRechargePaymentService<RechargePayment> _rechargePaymentService  = rechargePaymentService ?? throw new ArgumentNullException(nameof(rechargePaymentService));
        private readonly ILogger<RechargeController> _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        private readonly PaymentApiClient _paymentApiClient = paymentApiClient ?? throw new ArgumentNullException(nameof(paymentApiClient));

        /// <summary>
        /// Performs a Recharge transaction for given user and beneficiary.
        /// </summary>
        /// <param name="rechargePaymentModel">The recharge payment model.</param>
        /// <returns><![CDATA[Task<IActionResult>]]></returns>
        [HttpPost]
        [ProducesResponseType(typeof(DebitTransactionResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post(RechargePaymentModel rechargePaymentModel)
        {
            try
            {
                this._logger.LogInformation("Recharge transaction initiated.");

                if (!this.ModelState.IsValid)
                    return BadRequest(ModelState);

                var rechargePayment = rechargePaymentModel.AsEntity();

                // Check if all the data validations are success. If not, return validation error.
                var validationResult = await this._rechargePaymentService.ValidateRechargeRequest(rechargePayment);

                this._logger.LogInformation($"Recharge transaction validation result: {Convert.ToInt32(validationResult)}.");

                if (validationResult != RechargeValidationEnum.Success)
                    return this.ReturnValidationResponse(rechargePayment, validationResult);

                this._logger.LogInformation($"Fetching userID: {rechargePayment.UserID} balance information.");

                // Check if user has balance left to do debit transaction(external BankingPaymentService).
                var userBankingDetails = await this._paymentApiClient.GetUserBankBalanceAsync(rechargePayment.UserID);

                // If userID is null, means no UserBankAccount details are present.
                if (userBankingDetails?.UserID <= 0)
                {
                    this._logger.LogInformation($"No balance information available for userID: {rechargePayment.UserID}.");
                    return NotFound($"No balance information available for userID: {rechargePayment.UserID}. Please contact customer care.");
                }

                // Requirement: The user can only top up with an amount equal to or less than their balance.
                var canRecharge = userBankingDetails?.AvailableBalance >= (rechargePayment.Amount + rechargePayment.ServiceFee);

                // If recharge is not possible, return BadRequest.
                if (!canRecharge)
                {
                    _logger.LogInformation($"UserID: {rechargePayment.UserID} balance is low. Recharge Transaction not allowed.");
                   
                    return BadRequest(new { Message = $"UserID: {rechargePayment.UserID} balance is low. Recharge Transaction not allowed." });
                }

                // If recharge is not possible, then perform Debit transaction(external BankingPaymentService)
                var debitResponse = await this._paymentApiClient.DebitAmountAsync(rechargePayment);

                // If debit is successful, record and insert this transaction in RechargeApp database.
                if (debitResponse != null && debitResponse.IsSucess)
                {
                    this._logger.LogInformation($"Recharge transaction success for userID: {rechargePayment.UserID}.");

                    await this._rechargePaymentService.InsertRechargeHistory(rechargePayment);
                }

                // return debitResponse;
                return Ok(debitResponse);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            finally
            {
                this._logger.LogInformation("Recharge transaction completed.");
            }
        }
         
        /// <summary>
        /// Return Recharge model validation response.
        /// </summary>
        /// <param name="rechargePayment">The recharge payment.</param>
        /// <param name="result">The result.</param>
        /// <returns>An IActionResult</returns>
        private IActionResult ReturnValidationResponse(RechargePayment rechargePayment, RechargeValidationEnum result)
        {
            if (result == RechargeValidationEnum.InvalidTopupPlan)
                return BadRequest(new { Message = $"Topup amount doesn't exists in TopupPlanOptions. Please enter correct amount." });
            else if (result == RechargeValidationEnum.InvalidUser)
                return BadRequest(new { Message = $"User does not exists with userID: {rechargePayment.UserID}" });
            else if (result == RechargeValidationEnum.InvalidBeneficiary)
                return BadRequest(new { Message = $"Beneficiary does not exists with beneficiaryID: {rechargePayment.BeneficiaryID}" });
            else if (result == RechargeValidationEnum.MonthlyRechargeLimitExceeded)
                return BadRequest(new { Message = $"UserID: {rechargePayment.UserID} has already exceeded monthly recharge limit of {Convert.ToDecimal(CreditRulesEnum.VerifiedUserPerMonth)}. Transaction not allowed." });
            else if (result == RechargeValidationEnum.RechargeLimitExceeded)
                return BadRequest(new { Message = $"UserID: {rechargePayment.UserID} has already exceeded recharge limit for beneficiaryID: {rechargePayment.BeneficiaryID}. Transaction not allowed." });
            else if (result == RechargeValidationEnum.ValidationError)
                return BadRequest(new { Message = $"rechargePayment request is invalid." });

            else
                return BadRequest(new { Message = $"Error occurred: {result}" });
        }
    }
}