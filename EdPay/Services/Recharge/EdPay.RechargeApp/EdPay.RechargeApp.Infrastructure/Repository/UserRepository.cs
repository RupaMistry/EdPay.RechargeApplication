using EdPay.Common.Domain;
using EdPay.Common.Domain.Core;

namespace EdPay.RechargeApp.Infrastructure.Repository
{
    /// <summary>
    /// IRepository for User's entity.
    /// </summary>
    public class UserRepository(RechargeAppDBContext dbContext) : IRepository<User>
    {
        private readonly RechargeAppDBContext _rechargeAppContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <summary>
        /// Gets user details by userID.
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns>User entity</returns> 
        public async Task<User> GetAsync(int userID)
        {
            try
            {
                var user = await this._rechargeAppContext.Users.FirstOrDefaultAsync(u => u.UserID == userID);

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
                var userList = await this._rechargeAppContext.Users.Where(u => u.IsVerified).ToListAsync();

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

                await this._rechargeAppContext.Users.AddAsync(user);

                int rowsAffected = await this._rechargeAppContext.SaveChangesAsync();

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
             
            this._rechargeAppContext.Users.Update(user);

            int rowsAffected = await this._rechargeAppContext.SaveChangesAsync();

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
             
            this._rechargeAppContext.Users.Remove(user);

            int rowsAffected = await this._rechargeAppContext.SaveChangesAsync();

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