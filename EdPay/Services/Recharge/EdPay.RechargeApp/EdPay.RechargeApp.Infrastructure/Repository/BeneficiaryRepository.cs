using EdPay.Common.Domain;
using EdPay.Common.Domain.Core;

namespace EdPay.RechargeApp.Infrastructure.Repository
{
    /// <summary>
    /// IRepository for User's Beneficiary entity
    /// </summary>
    public class BeneficiaryRepository(RechargeAppDBContext dbContext) : IRepository<Beneficiary> 
    {
        private readonly RechargeAppDBContext _rechargeAppContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <summary>
        /// Gets beneficiary details by beneficiariesID.
        /// </summary>
        /// <param name="beneficiaryID">beneficiariesID</param>
        /// <returns>Beneficiary entity</returns> 
        public async Task<Beneficiary> GetAsync(int beneficiaryID)
        {
            try
            {
                var beneficiary = await this._rechargeAppContext.Beneficiaries.FirstOrDefaultAsync(u => u.BeneficiaryID == beneficiaryID);

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
                var list = await this._rechargeAppContext.Beneficiaries.ToListAsync();

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

                await this._rechargeAppContext.Beneficiaries.AddAsync(beneficiary);

                int rowsAffected = await this._rechargeAppContext.SaveChangesAsync();

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
        public async Task<int> UpdateAsync(Beneficiary entity)
        {
            if (entity == null) return -1;

            var beneficiary = await this.GetAsync(entity.BeneficiaryID);

            if (beneficiary == null) return -1;

            beneficiary.PhoneNumber = entity.PhoneNumber; 
             
            this._rechargeAppContext.Beneficiaries.Update(beneficiary);

            int rowsAffected = await this._rechargeAppContext.SaveChangesAsync();

            return rowsAffected;
        }

        /// <summary>
        /// Deletes a beneficiary entity.
        /// </summary>
        /// <param name="beneficiariesID">The beneficiaries ID.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> DeleteAsync(int beneficiaryID)
        {
            var beneficiary = await this.GetAsync(beneficiaryID);

            if (beneficiary == null) return -1;
             
            this._rechargeAppContext.Beneficiaries.Remove(beneficiary);

            int rowsAffected = await this._rechargeAppContext.SaveChangesAsync();

            return rowsAffected;
        }

        /// <summary>
        /// Checks if beneficiary is active or not.
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