namespace EdPay.RechargeApp.Application.Core
{
    /// <summary>
    /// IService for TopupPlans entity
    /// </summary>
    public interface ITopupPlanService<T> where T : Entity
    {
        /// <summary>
        /// Returns list of TopupPlans
        /// </summary>
        /// <returns>List of TopupPlans</returns>
        Task<IReadOnlyList<T>> GetTopupPlans();
    }
}