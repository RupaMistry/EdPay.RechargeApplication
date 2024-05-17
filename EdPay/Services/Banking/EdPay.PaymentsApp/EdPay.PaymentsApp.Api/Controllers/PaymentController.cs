using EdPay.PaymentsApp.Api.ViewModels;
using EdPay.PaymentsApp.Application.Core;
using EdPay.PaymentsApp.Domain;
using System.Net;

namespace EdPay.PaymentsApp.Api.Controllers
{
    /// <summary>
    /// Banking Payments API Controller.
    /// </summary>
    /// <param name="userBankAccountService"></param>
    /// <param name="transactionService"></param>
    /// <param name="logger"></param>
    [ApiController]
    [Route("api/payments")]
    public class PaymentController(
        IUserBankAccountService<UserBankAccount> userBankAccountService,
        ITransactionService<TransactionHistory> transactionService,
        ILogger<PaymentController> logger) : ControllerBase
    {
        private readonly IUserBankAccountService<UserBankAccount> _userBankAccountService =
            userBankAccountService ?? throw new ArgumentNullException(nameof(userBankAccountService));

        private readonly ITransactionService<TransactionHistory> _transactionService =
            transactionService ?? throw new ArgumentNullException(nameof(transactionService));

        private readonly ILogger<PaymentController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Returns current available balance details for given userID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>UserBankingDetails</returns>
        [HttpGet("balance")]
        [ProducesResponseType(typeof(UserBankingDetails), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserBalanceAsync(int userID)
        {
            if (userID <= 0)
                return BadRequest("Invalid userID value.");

            // Get user bank balance details 
            var userBankingDetails = await this._userBankAccountService.GetUserBankBalance(userID);

            //if userBankingDetails doesn't exist, log response.
            if ((userBankingDetails?.UserID == 0))
                _logger.LogInformation($"No user:{userID} found in database to fetch balance information.");
            else
                _logger.LogInformation($"UserBanking details for: {userID} returned successfully.");

            return Ok(userBankingDetails);
        }

        /// <summary>
        /// Performs a debit transaction on User bank account system.
        /// </summary>
        /// <param name="debitTransaction"></param>
        /// <returns>DebitTransactionResponse</returns>
        [HttpPost("debit")]
        [ProducesResponseType(typeof(DebitTransactionResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DebitTransactionAsync(DebitTransactionModel debitTransaction)
        {
            try
            {
                this._logger.LogInformation("Debit transaction initiated.");

                if (!this.ModelState.IsValid)
                    return BadRequest(ModelState);

                // Validate debitTransaction request and if valid, debit amount from UserBank balance.
                var debitResponse = await this._transactionService.DebitAmountAsync(debitTransaction.AsEntity());

                // If validation error, return BadResponse, else return success response.
                var responseMessage = this.GetValidationMessage(debitTransaction, debitResponse);

                _logger.LogInformation(responseMessage);

                return Ok(new
                {
                    TransactionID = debitResponse.TransactionID,
                    IsSucess = debitResponse.IsSuccess,
                    ErrorMessage = (!debitResponse.IsSuccess) ? responseMessage : string.Empty
                });
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            finally
            {
                this._logger.LogInformation("Debit transaction completed.");
            }
        }

        /// <summary>
        /// Performs a credit transaction on User bank account system.
        /// Not fully functional due to out of requirement scope :)
        /// </summary>
        /// <param name="creditTransaction"></param>
        /// <returns>CreditTransactionResponse</returns>
        [HttpPost("credit")]
        [ProducesResponseType(typeof(CreditTransactionResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreditTransactionAsync(CreditTransactionModel creditTransaction)
        {
            try
            {
                this._logger.LogInformation("Credit transaction initiated.");

                if (!this.ModelState.IsValid)
                    return BadRequest(ModelState);

                var debitResponse = await this._transactionService.CreditAmountAsync(creditTransaction.AsEntity());

                // If validation error, return BadResponse, else return success response.
                if (debitResponse.TransactionStatus == TransactionStatusEnum.InvalidUser)
                {
                    _logger.LogInformation($"No user:{creditTransaction.UserID} found in database.");
                    return NotFound(new { Message = $"User does not exist with ID: {creditTransaction.UserID}" });
                }
                else if (debitResponse.TransactionStatus == TransactionStatusEnum.Failure || !debitResponse.IsSuccess)
                {
                    _logger.LogInformation(
                        $"Error occured while performing credit. Reason: {debitResponse.ErrorMessage}.");
                    return BadRequest(new
                    {
                        Message =
                            $"Error occured while performing credit transaction. Reason: {debitResponse.ErrorMessage}."
                    });
                }
                else
                {
                    _logger.LogInformation(
                        $"Credit Transaction completed successfully. TransactionID: {debitResponse.TransactionID}.");

                    return Ok(new { TransactionID = debitResponse.TransactionID, IsSucess = debitResponse.IsSuccess });
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            finally
            {
                this._logger.LogInformation("Debit transaction completed.");
            }
        }

        private string GetValidationMessage(DebitTransactionModel debitTransaction,
            DebitTransactionResponse debitResponse)
        {
            string responseMessage;

            if (debitResponse.transactionStatus == TransactionStatusEnum.InvalidUser)
                responseMessage = $"User does not exist with ID: {debitTransaction.UserID}";
            else if (debitResponse.transactionStatus == TransactionStatusEnum.InvalidBeneficiary)
                responseMessage = $"Beneficiary does not exist with ID: {debitTransaction.BeneficiaryID}";
            else if (debitResponse.transactionStatus == TransactionStatusEnum.Failure || !debitResponse.IsSuccess)
                responseMessage =
                    $"Error occured while performing debit transaction. Reason: {debitResponse.ErrorMessage}.";
            else
                responseMessage =
                    $"Debit Transaction completed successfully. TransactionID: {debitResponse.TransactionID}.";
            return responseMessage;
        }
    }
}