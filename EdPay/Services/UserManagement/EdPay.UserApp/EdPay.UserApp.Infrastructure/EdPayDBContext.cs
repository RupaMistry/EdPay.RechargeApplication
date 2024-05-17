namespace EdPay.UserApp.Infrastructure
{
    /// <summary>
    /// EdPayUserApp db context 
    /// </summary>
    public class EdPayDBContext(DbContextOptions<EdPayDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Beneficiary> Beneficiaries { get; set; } = null!;

        /// <summary>
        /// Configures the schema needed for the EdPay context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}