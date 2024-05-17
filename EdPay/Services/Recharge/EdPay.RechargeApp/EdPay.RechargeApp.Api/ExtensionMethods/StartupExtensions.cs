namespace EdPay.RechargeApp.Api.ExtensionMethods
{
    /// <summary>
    /// Extension methods for IServiceCollection.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Setups and registers service instances
        /// </summary>
        /// <param name="services"></param> 
        public static void SetupServices(this IServiceCollection services)
        {
            // Repository and ApplicationServices
            services.AddScoped<ITopupPlanService<TopupPlan>, TopupPlanService>();
            services.AddScoped<ITopupPlanRepository<TopupPlan>, TopupPlanRepository>();

            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Beneficiary>, BeneficiaryRepository>();

            services.AddScoped<IRechargePaymentService<RechargePayment>, RechargePaymentService>();
            services.AddScoped<IRechargeHistoryRepository<RechargeHistory>, RechargeHistoryRepository>();
        }
    }
}