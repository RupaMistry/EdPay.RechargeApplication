using EdPay.Common.Domain;
using EdPay.PaymentsApp.Application.Core;

namespace EdPay.PaymentsApp.Application.Services
{
    /// <summary>
    /// The user bank account service.
    /// </summary>
    /// <param name="userAccountRepository">The user account repository.</param>
    public class UserBankAccountService(IUserBankAccountRepository<UserBankAccount> userAccountRepository) : IUserBankAccountService<UserBankAccount>
    { 
        private readonly IUserBankAccountRepository<UserBankAccount> _userAccountRepository = userAccountRepository;

        /// <summary>
        /// Get and return UserBankAccount for given userID
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<UserBankAccount>]]></returns>
        public async Task<UserBankAccount> GetAsync(int userID)
        {
            return await this._userAccountRepository.GetAsync(userID);
        }

        /// <summary>
        /// Gets all UserBankAccounts
        /// </summary>
        /// <returns><![CDATA[Task<IReadOnlyList<UserBankAccount>>]]></returns>
        public async Task<IReadOnlyList<UserBankAccount>> GetAllAsync()
        {
            return await this._userAccountRepository.GetAllAsync();
        }

        /// <summary>
        /// Creates a new UserBankAccount in system.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><![CDATA[Task<UserCreationEnum>]]></returns>
        public async Task<UserCreationEnum> CreateAsync(UserBankAccount entity)
        {
            return await this._userAccountRepository.CreateAsync(entity);
        }

        /// <summary>
        /// Updates UserBankAccount details.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> UpdateAsync(UserBankAccount entity)
        {
            return await this._userAccountRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// Deletes UserBankAccount from system
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> DeleteAsync(int userID)
        {
             return await this._userAccountRepository.DeleteAsync(userID);
        }

        /// <summary>
        /// Returns current available balance details for given userID.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<UserBankingDetails>]]></returns>
        public async Task<UserBankingDetails> GetUserBankBalance(int userID)
        {
            return await this._userAccountRepository.GetUserBankBalance(userID);
        }

        /// <summary>
        /// Checks if UserBankAccount exists or not.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <returns><![CDATA[Task<bool>]]></returns>
        public async Task<bool> Exists(int ID)
        {
            return await this._userAccountRepository.Exists(ID);
        }
    }
}