namespace EdPay.PaymentsApp.Infrastructure
{
    /// <summary>
    /// DBContext for PaymentApp DB.
    /// </summary>
    public class PaymentAppDBContext(DbContextOptions<PaymentAppDBContext> options) : DbContext(options)
    {
        public DbSet<UserBankAccount> UserBankAccounts { get; set; } = null!;

        public DbSet<TransactionPurpose> TransactionPurposes { get; set; } = null!;

        public DbSet<TransactionHistory> TransactionHistory { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Beneficiary> Beneficiaries { get; set; } = null!;

        /// <summary>
        /// Configures the schema needed for the PaymentApp context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionPurpose>().HasData(SeedTransactionPurposes);

            base.OnModelCreating(modelBuilder);
        }

        // Requirement: The user should be able to view available top-up options (AED 5, AED 10, AED 20, AED 30, AED 50, AED 75, AED 100). 
        private static readonly TransactionPurpose[] SeedTransactionPurposes =
        {
            new() { ID = 1, Description = "MobileRecharge" },
            new() { ID = 2, Description = "FamilyMaintenanceSavings" },
            new() { ID = 3, Description = "Taxes" },
        };
    }
}