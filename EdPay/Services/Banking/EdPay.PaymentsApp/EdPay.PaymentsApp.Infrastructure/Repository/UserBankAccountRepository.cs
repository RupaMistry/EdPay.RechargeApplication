namespace EdPay.PaymentsApp.Infrastructure.Repository
{
    /// <summary>
    /// IRepository for User's Beneficiary entity
    /// </summary>
    public class UserBankAccountRepository(PaymentAppDBContext dbContext) : IUserBankAccountRepository<UserBankAccount>
    {
        private readonly PaymentAppDBContext _paymentAppContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <summary>
        /// Gets userBankAccount details by userID.
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns>UserBankAccount entity</returns> 
        public async Task<UserBankAccount> GetAsync(int userID)
        {
            try
            {
                var userBankAccount = await this._paymentAppContext.UserBankAccounts.
                    FirstOrDefaultAsync(u => u.UserID == userID && u.IsAccountActive);

                return userBankAccount;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get UserBankAccounts list
        /// </summary>
        /// <returns><![CDATA[Task<IReadOnlyList<UserBankAccount>>]]></returns>
        public async Task<IReadOnlyList<UserBankAccount>> GetAllAsync()
        {
            try
            {
                var list = await this._paymentAppContext.UserBankAccounts.Where(u => u.IsAccountActive).ToListAsync();

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Registers a new user in system.
        /// </summary>
        /// <param name="userBankAccount"></param>
        /// <returns>Rows affected count</returns>
        public async Task<UserCreationEnum> CreateAsync(UserBankAccount userBankAccount)
        {
            try
            {
                if(userBankAccount == null) return UserCreationEnum.InvalidUserError;

                userBankAccount.RegistrationDate = DateTime.UtcNow;
                 
                await this._paymentAppContext.UserBankAccounts.AddAsync(userBankAccount);

                int rowsAffected = await this._paymentAppContext.SaveChangesAsync();

                return (rowsAffected <= 0) ? UserCreationEnum.InvalidUserError : UserCreationEnum.UserCreated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates a UserBankAccount entity.
        /// </summary>
        /// <param name="userBankAccount">The user bank account.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> UpdateAsync(UserBankAccount userBankAccount)
        {
            if (userBankAccount == null) return -1;

            var bankDetails = await this.GetAsync(userBankAccount.UserID);

            if (bankDetails == null) return -1;

            // Only user balance update is allowed. Not the EdPay card number or companyName used during registration.
            bankDetails.AvailableBalance = userBankAccount.AvailableBalance; 
             
            this._paymentAppContext.UserBankAccounts.Update(bankDetails);

            int rowsAffected = await this._paymentAppContext.SaveChangesAsync();

            return rowsAffected;
        }

        /// <summary>
        /// Deletes a UserBankAccount entity.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> DeleteAsync(int userID)
        {
            var userBankAccount = await this.GetAsync(userID);

            if (userBankAccount == null) return -1;
             
            this._paymentAppContext.UserBankAccounts.Remove(userBankAccount);

            int rowsAffected = await this._paymentAppContext.SaveChangesAsync();

            return rowsAffected;
        }

        /// <summary>
        /// Returns current available balance details for given userID.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<UserBankingDetails>]]></returns>
        public async Task<UserBankingDetails> GetUserBankBalance(int userID)
        {
            var bankingDetails = new UserBankingDetails(0, 0);

            // Check if userBankAccount exists or not.
            var userBankAccount = await this.GetAsync(userID);

            if (userBankAccount == null)
                return bankingDetails;

            // If exists, return current balance.
            bankingDetails = new UserBankingDetails(userBankAccount.UserID, userBankAccount.AvailableBalance);

            return bankingDetails;
        }

        /// <summary>
        /// Checks if userBankAccount exists in system or not.
        /// </summary>
        /// <param name="userID"></param> 
        /// <returns>Boolean</returns>
        public async Task<bool> Exists(int userID)
        {
            try
            {
                var userBankAccount = await this.GetAsync(userID);

                return (userBankAccount != null && userBankAccount.ID > 0);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}