using EdPay.Common.Domain;
using EdPay.PaymentsApp.Api.ViewModels;
using EdPay.PaymentsApp.Application.Core;
using System.Net;

namespace EdPay.PaymentsApp.Api.Controllers
{
    /// <summary>
    /// UserBankAccount API Controller.
    /// NOTE: This API is just for Testing purpose. 
    /// In a real scenario all verified back accounts would be either integrated through some Financial system or through Company application.
    /// </summary>
    [Route("api/bankAccounts")]
    [ApiController]
    public class UserBankAccountController(IUserBankAccountService<UserBankAccount> bankService, ILogger<UserBankAccountController> logger) : ControllerBase
    {
        private readonly IUserBankAccountService<UserBankAccount> _bankService = bankService ?? throw new ArgumentNullException(nameof(bankService));
        private readonly ILogger<UserBankAccountController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
         
        /// <summary>
        /// Returns UserBankAccount model for given userID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>UserBankAccount Model</returns>
        [HttpGet("{userID}")]
        [ProducesResponseType(typeof(UserBankAccount), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int userID)
        {
            if (userID <= 0)
                return BadRequest("Invalid userID value.");

            // Get userBankAccount details by userID
            var user = await this._bankService.GetAsync(userID);

            // if userBankAccountID is null, return NotFound() else success response.
            if (user == null)
            {
                _logger.LogInformation($"No userBankAccount found in database for userID:{userID}");
                return NotFound(new { Message = $"UserBankAccount does not exist for userID:{userID}" });
            }

            _logger.LogInformation($"UserBankAccount details for userID:{userID} returned successfully.");

            return Ok(user);
        }

        /// <summary>
        /// Creates a new user bank account record. 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>UserBankAccount</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserBankAccount), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] UserBankAccountModel model)
        {
            try
            {
                this._logger.LogInformation("UserBankAccount creation initiated.");

                if (!this.ModelState.IsValid)
                    return BadRequest(ModelState);

                var userBankAccount = model.AsEntity();

                // register new userBankAccount in system.
                var result = await this._bankService.CreateAsync(userBankAccount);

                // If validation error, return BadResponse, else return success response. 
                if (result == UserCreationEnum.InvalidUserError)
                    return BadRequest(new
                        { Message = "UserBankAccount creation failed! Please check details and try again." });

                _logger.LogInformation($"UserBankAccount created for userID:{userBankAccount.UserID} successfully.");

                return Ok(new { userBankAccountID = userBankAccount.ID });
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            finally
            {
                this._logger.LogInformation("UserBankAccount creation completed.");
            }
        }

        /// <summary>
        /// Deletes a UserBankAccount from system.
        /// </summary>
        /// <param name="userBankAccountID"></param> 
        [HttpDelete("{userBankAccountID}")]
        [ProducesResponseType(typeof(Int32), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAsync(int userBankAccountID)
        {
            try
            {
                this._logger.LogInformation("UserBankAccount deletion initiated.");

                if (userBankAccountID <= 0)
                    return BadRequest("Invalid userBankAccountID value.");

                var result = await this._bankService.DeleteAsync(userBankAccountID);

                // If validation error, return NotFound, else return success response. 
                if (result <= 0)
                {
                    _logger.LogInformation($"No userBankAccount:{userBankAccountID} found in database.");
                    return NotFound(new { Message = $"UserBankAccount does not exist with ID: {userBankAccountID}" });
                }

                return Ok(new { Message = "UserBankAccount deleted successfully!" });
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            finally
            {
                this._logger.LogInformation("UserBankAccount deletion completed.");
            }
        }
    }
}