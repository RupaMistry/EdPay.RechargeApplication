using EdPay.Common.Domain.Core;

namespace EdPay.RechargeApp.Infrastructure.Repository
{
    /// <summary>
    /// IRepository for RechargeHistory entity
    /// </summary>
    /// <param name="dbContext">The db context.</param>
    /// <param name="userRepository">The user repository.</param>
    public class RechargeHistoryRepository(RechargeAppDBContext dbContext, IRepository<User> userRepository)
        : IRechargeHistoryRepository<RechargeHistory>
    {
        private readonly RechargeAppDBContext _rechargeAppContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly IRepository<User> _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        /// <summary>
        /// Returns available recharge credit based on user's verified status.
        /// </summary>
        /// <param name="rechargePayment">The recharge payment.</param>
        /// <returns><![CDATA[Task<decimal>]]></returns>
        public async Task<decimal> GetAvailableRechargeCredit(RechargePayment rechargePayment)
        {
            var user = await this._userRepository.GetAsync(rechargePayment.UserID);

            var userVerified = user?.IsVerified;

            if (userVerified.Value)
                return await this.GetVerifiedUserAvailableCredit(rechargePayment);
            else
                return await this.GetNonVerifiedUserAvailableCredit(rechargePayment); 
        }

        /// <summary>
        /// Checks if user recharge limit has exceeded.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<bool>]]></returns>
        public async Task<bool> HasUserRechargeLimitExceeded(int userID)
        {
            var currentDate = DateTime.UtcNow;

            var transactions = this._rechargeAppContext.RechargeHistory.Where(
                r => r.UserID == userID &&
                r.TransactionDate.Month == currentDate.Month && r.TransactionDate.Year == currentDate.Year);

            var totalRechargedAmount = await transactions.SumAsync(r => r.Amount);

            // Requirement: The user can top up multiple beneficiaries but is limited to a total of AED 3,000 per month for all beneficiaries.
            return (totalRechargedAmount >= Convert.ToDecimal(CreditRulesEnum.VerifiedUserPerMonth));
        } 

        /// <summary>
        /// Inserts a new RechargeHistory record.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> InsertRechargeHistory(RechargeHistory entity)
        {
            try
            {
                if (entity == null) return -1;

                await this._rechargeAppContext.RechargeHistory.AddAsync(entity);

                int rowsAffected = await this._rechargeAppContext.SaveChangesAsync();

                return rowsAffected;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns sum of all transactions total.
        /// </summary>
        /// <param name="rechargePayment">The recharge payment.</param>
        /// <returns><![CDATA[Task<decimal>]]></returns>
        private async Task<decimal> GetAllTransactionsTotal(RechargePayment rechargePayment)
        {
            var currentDate = DateTime.UtcNow;

            var transactions = this._rechargeAppContext.RechargeHistory.Where(
                r => r.UserID == rechargePayment.UserID &&
                     r.BeneficiaryID == rechargePayment.BeneficiaryID &&
                     r.TransactionDate.Month == currentDate.Month && r.TransactionDate.Year == currentDate.Year);

            var totalRechargedAmount = await transactions.SumAsync(r => r.Amount);

            return totalRechargedAmount;
        }

        /// <summary>
        /// Get available credit amount for verified user.
        /// </summary>
        /// <param name="rechargePayment">The recharge payment.</param>
        /// <returns><![CDATA[Task<decimal>]]></returns>
        private async Task<decimal> GetVerifiedUserAvailableCredit(RechargePayment rechargePayment)
        {
            // Requirement: If a user is verified, they can top up a maximum of AED 500 per calendar month per beneficiary. 
            var totalRechargedAmount = await this.GetAllTransactionsTotal(rechargePayment);

            var availableCredit = (Convert.ToDecimal(CreditRulesEnum.VerifiedUser) - totalRechargedAmount);

            return availableCredit;
        }

        /// <summary>
        /// Get available credit for non verified user.
        /// </summary>
        /// <param name="rechargePayment">The recharge payment.</param>
        /// <returns><![CDATA[Task<decimal>]]></returns>
        private async Task<decimal> GetNonVerifiedUserAvailableCredit(RechargePayment rechargePayment)
        {
            // Requirement: If a user is not verified, they can top up a maximum of AED 1,000 per calendar month per beneficiary.
            var totalRechargedAmount = await this.GetAllTransactionsTotal(rechargePayment);

            var availableCredit = (Convert.ToDecimal(CreditRulesEnum.NonVerifiedUser) - totalRechargedAmount);

            return availableCredit;
        }

    }
}