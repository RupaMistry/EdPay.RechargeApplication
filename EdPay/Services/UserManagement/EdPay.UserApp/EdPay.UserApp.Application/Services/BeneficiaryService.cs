namespace EdPay.UserApp.Application.Services
{
    /// <summary>
    /// BeneficiaryService class.
    /// </summary>
    /// <param name="beneficiaryRepository">The beneficiary repository.</param>
    public class BeneficiaryService( IBeneficiaryRepository<Beneficiary> beneficiaryRepository) : IBeneficiaryService<Beneficiary>
    { 
        private readonly IBeneficiaryRepository<Beneficiary> _beneficiaryRepository = beneficiaryRepository ?? throw new ArgumentNullException(nameof(beneficiaryRepository));

        /// <summary>
        /// Returns beneficiary model for given beneficiaryID.
        /// </summary>
        /// <param name="beneficiaryID"></param>
        /// <returns>Returns a beneficiary</returns>
        public async Task<Beneficiary> GetAsync(int beneficiaryID)
        {
            return await _beneficiaryRepository.GetAsync(beneficiaryID);
        }

        /// <summary>
        /// Returns list of all active beneficiaries.
        /// </summary>
        /// <returns>List of Beneficiaries</returns>
        public async Task<IReadOnlyList<Beneficiary>> GetAllAsync()
        {
            return await _beneficiaryRepository.GetAllAsync();
        }

        /// <summary>
        /// Returns list of all active beneficiaries for given userID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>List of Beneficiaries</returns>
        public async Task<IReadOnlyList<Beneficiary>> GetAllAsync(int userID)
        {
            return await _beneficiaryRepository.GetAllAsync(userID);
        }

        /// <summary>
        /// Registers a new top-up beneficiary.
        /// </summary>
        /// <param name="beneficiary"></param>
        /// <returns>Rows affected count</returns>
        public async Task<UserCreationEnum> CreateAsync(Beneficiary beneficiary)
        { 
            return await _beneficiaryRepository.CreateAsync(beneficiary);
        }

        /// <summary>
        /// Update a beneficiary in system.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> UpdateAsync(Beneficiary entity)
        {
            return await _beneficiaryRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// Deletes a beneficiary from system.
        /// </summary>
        /// <param name="beneficiaryID"></param> 
        /// <returns>Rows affected count</returns>
        public async Task<int> DeleteAsync(int beneficiaryID)
        {
            return await _beneficiaryRepository.DeleteAsync(beneficiaryID);
        } 

        /// <summary>
        /// Returns count of all active beneficiaries for given userID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>Active count</returns>
        public async Task<int?> GetActiveBeneficiariesCount(int userID)
        {
            return await _beneficiaryRepository.GetActiveBeneficiariesCount(userID);
        }

        /// <summary>
        /// Checks if beneficiary exists or not.
        /// </summary>
        /// <param name="ID">The ID.</param>
        /// <returns><![CDATA[Task<bool>]]></returns>
        public async Task<bool> Exists(int ID)
        {
            return await _beneficiaryRepository.Exists(ID);
        }
    }
}