namespace EdPay.UserApp.Api.Controllers
{
    /// <summary>
    /// Beneficiary API Controller
    /// </summary>
    /// <param name="beneficiaryService"></param>
    /// <param name="publishEndpoint"></param> 
    /// <param name="logger"></param> 
    [ApiController]
    [Route("api/beneficiary")]
    public class BeneficiaryController(IBeneficiaryService<Beneficiary> beneficiaryService, IPublishEndpoint publishEndpoint, ILogger<BeneficiaryController> logger) : ControllerBase
    {
        private readonly IBeneficiaryService<Beneficiary> _beneficiaryService = beneficiaryService ?? throw new ArgumentNullException(nameof(beneficiaryService));
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        private readonly ILogger<BeneficiaryController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Returns beneficiary model for given beneficiaryID.
        /// </summary>
        /// <param name="beneficiaryID"></param>
        /// <returns>Beneficiary Model</returns>
        [HttpGet("{beneficiaryID}")]
        [ProducesResponseType(typeof(Beneficiary), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAsync(int beneficiaryID)
        {
            if (beneficiaryID <= 0)
                return BadRequest("Invalid beneficiaryID value.");

            // Get beneficiary details by beneficiaryID
            var beneficiary = await this._beneficiaryService.GetAsync(beneficiaryID);

            // if beneficiary is null, return NotFound() else success response.
            if (beneficiary == null)
            {
                _logger.LogInformation($"No beneficiary:{beneficiaryID} found in database.");
                return NotFound(new { Message = $"Beneficiary does not exist with ID: {beneficiaryID}" });
            }

            _logger.LogInformation($"Beneficiary details for: {beneficiaryID} returned successfully.");
            return Ok(beneficiary);
        }

        /// <summary>
        /// Gets a list of available beneficiaries
        /// </summary>
        [HttpGet]
        [Route("list")]
        [ProducesResponseType(typeof(IReadOnlyList<Beneficiary>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<IReadOnlyList<Beneficiary>>> GetAllAsync(int userID)
        {
            if (userID <= 0)
                return BadRequest("Invalid userID value."); 

            // Fetch and return all active top-up beneficiaries
            var beneficiaries = await _beneficiaryService.GetAllAsync(userID);

            // if list is empty, return NotFound() else success response.
            if (beneficiaries == null || beneficiaries?.Count <= 0)
            {
                _logger.LogInformation("No beneficiaries found in database");
                return NotFound(new { Message = $"No beneficiaries found in database with userID: {userID}" });
            }

            _logger.LogInformation("Beneficiaries list returned successfully.");
            return Ok(new { Data = beneficiaries });
        }

        /// <summary>
        /// Registers a new beneficiary in system.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>User creation status enum</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] BeneficiaryModel model)
        {
            try
            {
                this._logger.LogInformation("Beneficiary creation initiated.");

                if (!this.ModelState.IsValid)
                    return BadRequest(ModelState);

                var beneficiary = model.AsEntity();

                // registers a new beneficiary in system
                var result = await this._beneficiaryService.CreateAsync(beneficiary);

                // If validation error, return BadResponse, else return success response.
                if (result == UserCreationEnum.BeneficiaryLimitExceeded)
                    return BadRequest(new
                        { Message = $"Only {Constants.BeneficiaryLimit} active top-up beneficiaries are allowed." });
                if (result == UserCreationEnum.InvalidUserID)
                    return BadRequest(new
                    {
                        Message =
                            $"User does not exist with userID: {beneficiary.UserID}. Please specify a valid userID to add beneficiary."
                    });
                if (result == UserCreationEnum.UserNameExists)
                    return BadRequest(new
                        { Message = $"Beneficiary already exists with nickName: {beneficiary.NickName}" });
                else if (result == UserCreationEnum.PhoneNumberExists)
                    return BadRequest(new
                        { Message = $"Beneficiary already exists with phoneNumber: {beneficiary.PhoneNumber}" });
                else if (result == UserCreationEnum.InvalidUserError)
                    return BadRequest(new
                        { Message = "Beneficiary creation failed! Please check beneficiary details and try again." });
                else
                {
                    this._logger.LogInformation(
                        "MassTransit: Publish messages to Consumers for new beneficiary creation.");

                    // MassTransit: Publish messages to Consumers for new beneficiary creation.
                    var messages = new List<object>
                    {
                        new RechargeBeneficiaryCreated(beneficiary.UserID, beneficiary.ID, beneficiary.PhoneNumber),
                        new PaymentBeneficiaryCreated(beneficiary.UserID, beneficiary.ID)
                    };

                    // Newly added Beneficiary is always active so no flag is sent to consumers.
                    await this._publishEndpoint.PublishBatch(messages.AsEnumerable());

                    return CreatedAtAction(nameof(GetAsync), new { beneficiaryID = beneficiary.ID }, beneficiary);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            finally
            {
                this._logger.LogInformation("Beneficiary creation completed.");
            }
        }

        /// <summary>
        /// Deactivates a beneficiary in system
        /// </summary>
        /// <param name="beneficiaryID"></param> 
        [HttpDelete("{beneficiaryID}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync(int beneficiaryID)
        {
            try
            {
                this._logger.LogInformation("Beneficiary deactivation initiated.");

                if (beneficiaryID <= 0)
                    return BadRequest("Invalid beneficiaryID value.");

                // deactivates a beneficiary in system
                var result = await this._beneficiaryService.DeleteAsync(beneficiaryID);

                // If validation error, return BadResponse, else return success response. 
                if (result == Constants.InvalidBeneficiary)
                    return NotFound(new { Message = $"Beneficiary does not exist with ID: {beneficiaryID}" });
                else
                {
                    this._logger.LogInformation("MassTransit: Publish messages to Consumers for beneficiary deletion.");

                    // MassTransit: Publish messages to Consumers for beneficiary deletion.
                    var messages = new List<object>
                    {
                        new RechargeBeneficiaryDeleted(beneficiaryID),
                        new PaymentBeneficiaryDeleted(beneficiaryID)
                    };

                    await this._publishEndpoint.PublishBatch(messages.AsEnumerable());

                    return Ok(new { Message = "Beneficiary deactivated successfully!" });
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            finally
            {
                this._logger.LogInformation("Beneficiary deactivation completed.");
            }
        }

        /// <summary>
        /// Returns count of all active beneficiaries in system.
        /// </summary>
        /// <returns>Count</returns>
        [HttpGet]
        [Route("activeCount")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)] 
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetActiveBeneficiariesCount(int userID)
        {
            if (userID <= 0)
                return BadRequest("Invalid userID value.");

            // Fetch the verification status for user.
            var count = await this._beneficiaryService.GetActiveBeneficiariesCount(userID);

            // if count is null, means no user exist.
            if (count == null)
            {
                _logger.LogInformation($"No user:{userID} found in database.");
                return NotFound(new { Message = $"User does not exist with ID: {userID}" });
            }

            _logger.LogInformation($"Active beneficiaries count for user: {userID} is: {count}.");

            return Ok(new { UserID = userID, ActiveCount = count });
        }
    }
}