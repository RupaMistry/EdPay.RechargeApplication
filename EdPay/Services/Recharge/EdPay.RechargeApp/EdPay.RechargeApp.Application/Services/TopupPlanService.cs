namespace EdPay.RechargeApp.Application.Services
{
    /// <summary>
    /// Service for TopupPlans entity
    /// </summary>
    /// <param name="repository"></param>
    public class TopupPlanService(ITopupPlanRepository<TopupPlan> repository) : ITopupPlanService<TopupPlan>
    {
        private readonly ITopupPlanRepository<TopupPlan> planRepository = repository;

        /// <summary>
        /// Returns list of TopupPlans
        /// </summary>
        /// <returns>List of TopupPlans</returns>
        public async Task<IReadOnlyList<TopupPlan>> GetTopupPlans()
        {
            return await planRepository.GetTopupPlans();
        }
    }
}