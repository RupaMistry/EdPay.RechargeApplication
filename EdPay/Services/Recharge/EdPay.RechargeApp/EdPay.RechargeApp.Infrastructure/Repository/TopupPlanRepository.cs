namespace EdPay.RechargeApp.Infrastructure.Repository
{
    /// <summary>
    /// Repository for TopupPlans entity
    /// </summary>
    /// <param name="dbContext"></param>
    public class TopupPlanRepository(RechargeAppDBContext dbContext) : ITopupPlanRepository<TopupPlan>
    {
        private readonly RechargeAppDBContext _rechargeAppContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <summary>
        /// Returns list of TopupPlans
        /// </summary>
        /// <returns>List of TopupPlans</returns>
        public async Task<IReadOnlyList<TopupPlan>> GetTopupPlans()
        {
            // Requirement: The user should be able to view available top-up options
            var plans = await this._rechargeAppContext.TopupPlans
                .Where(t => t.IsActive)
                .ToListAsync();

            return plans;
        }

        /// <summary>
        /// Checks if plan exists or not.
        /// </summary>
        /// <returns>Boolean</returns>
        public async Task<bool> Exists(decimal amount)
        {
            try
            {
                var plan = await this._rechargeAppContext.TopupPlans.FirstOrDefaultAsync
                  (u => u.Amount == amount && u.IsActive);

                return (plan != null && plan.ID > 0);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}