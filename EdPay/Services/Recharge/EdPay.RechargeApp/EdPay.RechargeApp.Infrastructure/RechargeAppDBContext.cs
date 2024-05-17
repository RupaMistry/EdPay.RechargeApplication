using EdPay.Common.Domain;

namespace EdPay.RechargeApp.Infrastructure
{
    /// <summary>
    /// DBContext for RechargeApp DB.
    /// </summary>
    public class RechargeAppDBContext(DbContextOptions<RechargeAppDBContext> options) : DbContext(options)
    {
        public DbSet<TopupPlan> TopupPlans { get; set; } = null!;

        public DbSet<CreditRule> CreditRules { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Beneficiary> Beneficiaries { get; set; } = null!;

        public DbSet<RechargeHistory> RechargeHistory { get; set; } = null!;


        /// <summary>
        /// Configures the schema needed for the RechargeApp context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TopupPlan>().HasData(SeedTopupPlans);

            modelBuilder.Entity<CreditRule>().HasData(SeedCreditRules);

            base.OnModelCreating(modelBuilder);
        }

        private static readonly TopupPlan[] SeedTopupPlans =
        {
            new() { ID=1, Amount=5, Currency=Currency.UAE, PlanDescription="AED 5" , IsActive = true },
            new() { ID=2, Amount=10, Currency=Currency.UAE, PlanDescription="AED 10", IsActive = true },
            new() { ID=3, Amount=20, Currency=Currency.UAE, PlanDescription="AED 20", IsActive = true },
            new() { ID=4, Amount=30, Currency=Currency.UAE, PlanDescription="AED 30", IsActive = true },
            new() { ID=5, Amount=50, Currency=Currency.UAE, PlanDescription="AED 50", IsActive = true },
            new() { ID=6, Amount=75, Currency=Currency.UAE, PlanDescription="AED 75", IsActive = true },
            new() { ID=7, Amount=100, Currency=Currency.UAE, PlanDescription="AED 100", IsActive = true }
        };

        private static readonly CreditRule[] SeedCreditRules =
        { 
            new() { ID=1,  Description="NonVerifiedUser", AmountLimit = 1000 , PerBeneficiary = true},
            new() { ID=2,  Description="VerifiedUser", AmountLimit = 500 , PerBeneficiary = true },
            new() { ID=3,  Description="VerifiedUserPerMonth" , AmountLimit = 3000 , PerBeneficiary = false},
        };
    }
}