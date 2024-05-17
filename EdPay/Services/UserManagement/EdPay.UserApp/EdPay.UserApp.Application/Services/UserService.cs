namespace EdPay.UserApp.Application.Services
{
    /// <summary>
    /// UserService class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    public class UserService(IUserRepository<User> userRepository) : IUserService<User>
    { 
        private readonly IUserRepository<User> _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        /// <summary>
        /// Gets user details by userID.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<User>]]></returns>
        public async Task<User> GetAsync(int userID)
        {
            return await this._userRepository.GetAsync(userID);
        }

        /// <summary>
        /// Gets all users list.
        /// </summary>
        /// <returns><![CDATA[Task<IReadOnlyList<User>>]]></returns>
        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await this._userRepository.GetAllAsync();
        }

        /// <summary>
        /// Registers a new user in system.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><![CDATA[Task<UserCreationEnum>]]></returns>
        public async Task<UserCreationEnum> CreateAsync(User entity)
        {
            return await this._userRepository.CreateAsync(entity);
        }

        /// <summary>
        /// Updates a User entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> UpdateAsync(User entity)
        {
            return await this._userRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// Deletes a User entity.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> DeleteAsync(int userID)
        {
             return await this._userRepository.DeleteAsync(userID);
        }

        /// <summary>
        /// Checks if user exists in system or not.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<bool>]]></returns>
        public async Task<bool> Exists(int userID)
        {
            return await this._userRepository.Exists(userID);
        }

        /// <summary>
        /// Checks if user is verified or not.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<bool?>]]></returns>
        public async Task<bool?> IsVerified(int userID)
        {
            return await this._userRepository.IsVerified(userID);
        }
    }
}