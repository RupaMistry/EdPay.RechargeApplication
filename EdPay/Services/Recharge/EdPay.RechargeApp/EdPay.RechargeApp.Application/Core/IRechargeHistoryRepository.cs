namespace EdPay.RechargeApp.Application.Core
{
    /// <summary>
    /// IRepository for RechargeHistory entity
    /// </summary>
    /// <typeparam name="T"/>
    public interface IRechargeHistoryRepository<T> where T : Entity
    {
        /// <summary>
        /// Checks if user recharge limit has exceeded.
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<bool>]]></returns>
        Task<bool> HasUserRechargeLimitExceeded(int userID);

        /// <summary>
        /// Returns available recharge credit based on user's verified status.
        /// </summary>
        /// <param name="rechargePayment">The recharge payment.</param>
        /// <returns><![CDATA[Task<decimal>]]></returns>
        Task<decimal> GetAvailableRechargeCredit(RechargePayment rechargePayment);

        /// <summary>
        /// Inserts a new RechargeHistory record.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        Task<int> InsertRechargeHistory(T entity);
    }
}