namespace EdPay.UserApp.Api.Controllers
{
    /// <summary>
    /// User API Controller
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="logger"></param> 
    [ApiController]
    [Route("api/user")]
    public class UserController(IUserService<User> userService, IPublishEndpoint publishEndpoint, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IUserService<User> _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        private readonly ILogger<UserController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Returns user model for given userID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>User Model</returns>
        [HttpGet("{userID}")]
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAsync(int userID)
        {
            if (userID <= 0)
                return BadRequest("Invalid userID value.");

            // Get user details by userID
            var user = await this._userService.GetAsync(userID);

            // if user is null, return NotFound() else success response.
            if (user == null)
            {
                _logger.LogInformation($"No user:{userID} found in database.");
                return NotFound(new { Message = $"User does not exist with ID: {userID}" });
            }

            _logger.LogInformation($"User details for: {userID} returned successfully.");
            return Ok(user);
        }

        /// <summary>
        /// Registers a new user in system
        /// </summary>
        /// <param name="UserModel"></param>
        /// <returns>User creation status enum</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] UserModel model)
        {
            try
            {
                this._logger.LogInformation("User creation initiated.");

                if (!this.ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = model.AsEntity();

                // register new user in system
                var result = await this._userService.CreateAsync(user);

                // If validation error, return BadResponse, else return success response.
                if (result == UserCreationEnum.UserNameExists)
                    return BadRequest(new { Message = $"User already exists with userName: {user.UserName}" });
                else if (result == UserCreationEnum.EmailIDExists)
                    return BadRequest(new { Message = $"User already exists with emailID: {user.EmailAddress}" });
                else if (result == UserCreationEnum.PhoneNumberExists)
                    return BadRequest(new { Message = $"User already exists with phoneNumber: {user.PhoneNumber}" });
                else if (result == UserCreationEnum.InvalidUserError)
                    return BadRequest(
                        new { Message = "User creation failed! Please check user details and try again." });
                else
                {
                    this._logger.LogInformation(
                        "MassTransit: Publish messages to Consumers for new user creation.");

                    // MassTransit: Publish messages to Consumers for new user creation.
                    var messages = new List<object>
                    {
                        new RechargeUserCreated(user.ID, user.IsVerified),
                        new PaymentUserCreated(user.ID, user.IsVerified)
                    };

                    await this._publishEndpoint.PublishBatch(messages.AsEnumerable());

                    return CreatedAtAction(nameof(GetAsync), new { userID = user.ID }, user);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            finally
            {
                this._logger.LogInformation("User creation completed.");
            }
        }

        /// <summary>
        /// Returns user verification status.
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        [Route("status")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetVerificationStatusAsync(int userID)
        {
            if (userID <= 0)
                return BadRequest("Invalid userID value.");

            // Fetch the verification status for user.
            var status = await this._userService.IsVerified(userID);

            // if status is null, means no user exist.
            if (status == null)
            {
                _logger.LogInformation($"No user:{userID} found in database.");
                return NotFound(new { Message = $"User does not exist with ID: {userID}" });
            }

            _logger.LogInformation($"User verification status: {status}.");

            return Ok(new { UserID = userID, IsVerified = status });
        }

        /// <summary>
        /// Deletes a user from system.
        /// </summary>
        /// <param name="userID"></param> 
        [HttpDelete("{userID}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync(int userID)
        {
            try
            {
                this._logger.LogInformation("User deletion initiated.");

                if (userID <= 0)
                    return BadRequest("Invalid userID value.");

                // deactivates a beneficiary in system
                var result = await this._userService.DeleteAsync(userID);

                // If validation error, return BadResponse, else return success response. 
                if (result == Constants.InvalidUser)
                    return NotFound(new { Message = $"User does not exist with ID: {userID}" });
                else
                {
                    this._logger.LogInformation("MassTransit: Publish messages to Consumers for new user deletion.");

                    // MassTransit: Publish messages to Consumers for new user deletion.
                    var messages = new List<object>
                    {
                        new RechargeUserDeleted(userID),
                        new PaymentUserDeleted(userID)
                    };

                    await this._publishEndpoint.PublishBatch(messages.AsEnumerable());

                    return Ok(new { Message = "User deleted successfully!" });
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            finally
            {
                this._logger.LogInformation("User deletion completed.");
            }
        }
    }
}