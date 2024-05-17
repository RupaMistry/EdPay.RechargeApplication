namespace EdPay.PaymentsApp.Application.Core
{
    /// <summary>
    /// The user bank account service interface.
    /// </summary>
    /// <typeparam name="T"/>
    public interface IUserBankAccountService<T> : IService<T> where T : Entity
    {
        /// <summary>
        /// Returns current available balance details for given userID.
        /// </summary>
        /// <param name="userID">The debit transaction request.</param>
        /// <returns><![CDATA[Task<UserBankingDetails>]]></returns>
        Task<UserBankingDetails> GetUserBankBalance(int userID);
    }
}
