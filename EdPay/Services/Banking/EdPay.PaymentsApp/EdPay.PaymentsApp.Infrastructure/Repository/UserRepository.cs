namespace EdPay.PaymentsApp.Infrastructure.Repository
{
    /// <summary>
    /// IRepository for User's Beneficiary entity
    /// </summary>
    public class UserRepository(PaymentAppDBContext dbContext) : IRepository<User>
    {
        private readonly PaymentAppDBContext _paymentAppDBContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <summary>
        /// Gets user details by userID.
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns>User entity</returns> 
        public async Task<User> GetAsync(int userID)
        {
            try
            {
                var user = await this._paymentAppDBContext.Users.FirstOrDefaultAsync(u => u.UserID == userID);

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
                var userList = await this._paymentAppDBContext.Users.Where(u => u.IsVerified).ToListAsync();

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

                await this._paymentAppDBContext.Users.AddAsync(user);

                int rowsAffected = await this._paymentAppDBContext.SaveChangesAsync();

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

            var user = await this.GetAsync(entity.UserID);

            if (user == null) return -1;

            // Update only allows IsVerified flag. Not the phone number or emailaddress used during registration.
            user.IsVerified = entity.IsVerified;
             
            this._paymentAppDBContext.Users.Update(user);

            int rowsAffected = await this._paymentAppDBContext.SaveChangesAsync();

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

            if (user == null) return -1;
             
            this._paymentAppDBContext.Users.Remove(user);

            int rowsAffected = await this._paymentAppDBContext.SaveChangesAsync();

            return rowsAffected;
        }

        /// <summary>
        /// Checks if user exists in system or not.
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
    }
}