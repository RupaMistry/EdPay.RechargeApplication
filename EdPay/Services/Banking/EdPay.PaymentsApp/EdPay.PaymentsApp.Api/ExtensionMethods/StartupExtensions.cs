using EdPay.PaymentsApp.Application.Core;
using EdPay.PaymentsApp.Application.Services;

namespace EdPay.PaymentsApp.Api.ExtensionMethods
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
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Beneficiary>, BeneficiaryRepository>();

            services.AddScoped<IUserBankAccountRepository<UserBankAccount>, UserBankAccountRepository>();
            services.AddScoped<IUserBankAccountService<UserBankAccount>, UserBankAccountService>();

            services.AddScoped<ITransactionService<TransactionHistory>, TransactionService>();
            services.AddScoped<ITransactionRepository<TransactionHistory>, TransactionRepository>();
        }
    }
}