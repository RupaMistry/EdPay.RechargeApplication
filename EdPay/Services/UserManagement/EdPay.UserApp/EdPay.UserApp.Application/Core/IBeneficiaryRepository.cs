namespace EdPay.UserApp.Application.Core
{
    /// <summary>
    /// IRepository for User's Beneficiary entity.
    /// </summary>
    public interface IBeneficiaryRepository<T> : IRepository<T> where T : Entity
    {
        /// <summary>
        /// Returns list of all active beneficiaries for given userID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>List of Beneficiaries</returns>
        Task<IReadOnlyList<T>> GetAllAsync(int userID);


        /// <summary>
        /// Returns count of all active beneficiaries for given userID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>Active count</returns>
        Task<int?> GetActiveBeneficiariesCount(int userID);
    }
}