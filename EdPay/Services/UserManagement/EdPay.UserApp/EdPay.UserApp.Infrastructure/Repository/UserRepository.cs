namespace EdPay.UserApp.Infrastructure.Repository
{
    /// <summary>
    /// IRepository for User's Beneficiary entity
    /// </summary>
    public class UserRepository(EdPayDBContext dbContext) : IUserRepository<User>
    {
        private readonly EdPayDBContext _edPayAppContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <summary>
        /// Gets user details by userID.
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns>User entity</returns> 
        public async Task<User> GetAsync(int userID)
        {
            try
            {
                var user = await this._edPayAppContext.Users.FirstOrDefaultAsync(u => u.ID == userID);

                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all users list.
        /// </summary>
        /// <returns><![CDATA[Task<IReadOnlyList<User>>]]></returns>
        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            try
            {
                var userList = await this._edPayAppContext.Users.Where(u => u.IsVerified).ToListAsync();

                return userList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Registers a new user in system.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Rows affected count</returns>
        public async Task<UserCreationEnum> CreateAsync(User user)
        {
            try
            {
                if (user == null) return UserCreationEnum.InvalidUserError;

                // Check if user exists by userName
                var userExists = await this._edPayAppContext.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);

                // If exists, return duplicate userName error
                if (userExists != null)
                    return UserCreationEnum.UserNameExists;

                // Check if user exists by emailID
                var emailExists = await this._edPayAppContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == user.EmailAddress);

                // If exists, return duplicate emailID error
                if (emailExists != null)
                    return UserCreationEnum.EmailIDExists;

                // Check if user exists by phone number
                var phoneExists = await this._edPayAppContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == user.PhoneNumber);

                // If exists, return duplicate emailID error
                if (phoneExists != null)
                    return UserCreationEnum.PhoneNumberExists;

                user.CreatedDate = DateTime.UtcNow;
                user.UserName = user.UserName.Replace(" ", "");

                await this._edPayAppContext.Users.AddAsync(user);

                int rowsAffected = await this._edPayAppContext.SaveChangesAsync();

                return (rowsAffected <= 0) ? UserCreationEnum.InvalidUserError : UserCreationEnum.UserCreated;
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Updates a User entity.
        /// </summary>
        /// <param name="entity">User</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> UpdateAsync(User entity)
        {
            if (entity == null) return -1; 

            var user = await this.GetAsync(entity.ID);

            if (user == null)
                return Constants.InvalidUser;

            // Update only allows userName and nationality. Not the phone number or emailaddress used during registration.
            user.UserName = entity.UserName; 
            user.Nationality = entity.Nationality;
             
            this._edPayAppContext.Users.Update(user);

            int rowsAffected = await this._edPayAppContext.SaveChangesAsync();

            return rowsAffected;
        }

        /// <summary>
        /// Deletes a User entity.
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> DeleteAsync(int userID)
        {
            var user = await this.GetAsync(userID);

            if (user == null) 
                return Constants.InvalidUser;
              
            this._edPayAppContext.Users.Remove(user);

            int rowsAffected = await this._edPayAppContext.SaveChangesAsync();

            return rowsAffected;
        }

        /// <summary>
        /// Checks if user exists in system.
        /// </summary>
        /// <param name="userID"></param> 
        /// <returns>Boolean</returns>
        public async Task<bool> Exists(int userID)
        {
            try
            {
                var user = await this.GetAsync(userID);

                return (user != null && user.ID > 0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if user is verified or not.
        /// </summary>
        /// <param name="userID"></param> 
        /// <returns>Boolean</returns>
        public async Task<bool?> IsVerified(int userID)
        {
            try
            {
                var user = await this.GetAsync(userID);

                return user?.IsVerified;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}