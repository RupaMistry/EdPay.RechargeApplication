namespace EdPay.RechargeApp.Application.Core
{
    /// <summary>
    /// IRepository for TopupPlans entity
    /// </summary>
    public interface ITopupPlanRepository<T> where T : Entity
    {
        /// <summary>
        /// Returns list of TopupPlans
        /// </summary>
        /// <returns>List of TopupPlans</returns>
        Task<IReadOnlyList<T>> GetTopupPlans();

        /// <summary>
        /// Checks if TopupPlan exist or not.
        /// </summary>
        /// <returns>Boolean</returns>
        Task<bool> Exists(decimal amount);
    }
}