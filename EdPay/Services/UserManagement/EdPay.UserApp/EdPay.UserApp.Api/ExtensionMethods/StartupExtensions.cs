namespace EdPay.UserApp.Api.ExtensionMethods
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
            services.AddScoped<IUserService<User>, UserService>();
            services.AddScoped<IUserRepository<User>, UserRepository>();

            services.AddScoped<IBeneficiaryService<Beneficiary>, BeneficiaryService>();
            services.AddScoped<IBeneficiaryRepository<Beneficiary>, BeneficiaryRepository>();
        }
    }
}