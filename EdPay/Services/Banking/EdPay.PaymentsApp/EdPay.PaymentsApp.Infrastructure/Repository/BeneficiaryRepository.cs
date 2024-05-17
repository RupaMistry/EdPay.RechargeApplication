namespace EdPay.PaymentsApp.Infrastructure.Repository
{
    /// <summary>
    /// IRepository for User's Beneficiary entity
    /// </summary>
    public class BeneficiaryRepository(PaymentAppDBContext dbContext) : IRepository<Beneficiary>
    { 
        private readonly PaymentAppDBContext _paymentAppContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <summary>
        /// Gets beneficiary details by beneficiariesID.
        /// </summary>
        /// <param name="beneficiariesID">beneficiariesID</param>
        /// <returns>Beneficiary entity</returns> 
        public async Task<Beneficiary> GetAsync(int beneficiariesID)
        {
            try
            {
                var beneficiary = await this._paymentAppContext.Beneficiaries.FirstOrDefaultAsync(u => u.BeneficiaryID == beneficiariesID);

                return beneficiary;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns list of all active beneficiaries for given userID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>List of Beneficiaries</returns>
        public async Task<IReadOnlyList<Beneficiary>> GetAllAsync()
        {
            try
            {
                var list = await this._paymentAppContext.Beneficiaries.ToListAsync();

                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a new beneficiary record in system.
        /// </summary>
        /// <param name="beneficiary"></param>
        /// <returns>Rows affected count</returns>
        public async Task<UserCreationEnum> CreateAsync(Beneficiary beneficiary)
        {
            try
            {
                if (beneficiary == null) return UserCreationEnum.InvalidUserError;

                await this._paymentAppContext.Beneficiaries.AddAsync(beneficiary);

                int rowsAffected = await this._paymentAppContext.SaveChangesAsync();

                return (rowsAffected <= 0) ? UserCreationEnum.InvalidUserError : UserCreationEnum.UserCreated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update a beneficiary entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns><![CDATA[Task<int>]]></returns>
        public Task<int> UpdateAsync(Beneficiary entity)
        {
            throw new NotImplementedException("No updates allowed for Beneficiary entity.");
        }

        /// <summary>
        /// Deletes a beneficiary entity.
        /// </summary>
        /// <param name="beneficiariesID">The beneficiaries ID.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> DeleteAsync(int beneficiariesID)
        {
            var beneficiary = await this.GetAsync(beneficiariesID);

            if (beneficiary == null) return -1;
             
            this._paymentAppContext.Beneficiaries.Remove(beneficiary);

            var rowsAffected = await this._paymentAppContext.SaveChangesAsync();

            return rowsAffected;
        }

        /// <summary>
        /// Checks if beneficiary exists in system.
        /// </summary>
        /// <param name="beneficiaryID"></param> 
        /// <returns>Boolean</returns>
        public async Task<bool> Exists(int beneficiaryID)
        {
            try
            {
                var beneficiary = await this.GetAsync(beneficiaryID);

                return (beneficiary != null && beneficiary.ID > 0);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}