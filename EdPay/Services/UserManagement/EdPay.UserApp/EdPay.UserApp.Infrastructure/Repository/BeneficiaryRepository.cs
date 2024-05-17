namespace EdPay.UserApp.Infrastructure.Repository
{
    /// <summary>
    /// Repository for UserMessage entity
    /// </summary>
    /// <param name="dbContext"></param>
    public class BeneficiaryRepository(EdPayDBContext dbContext, IUserRepository<User> userRepository) : IBeneficiaryRepository<Beneficiary>
    {
        private readonly EdPayDBContext _edPayAppContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly IUserRepository<User> _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        /// <summary>
        /// Returns beneficiary model for given beneficiaryID.
        /// </summary>
        /// <param name="beneficiaryID"></param>
        /// <returns>Returns a beneficiary</returns>
        public async Task<Beneficiary> GetAsync(int beneficiaryID)
        {
            try
            {
                var beneficiary = await this._edPayAppContext.Beneficiaries
                .FirstOrDefaultAsync(m => m.ID == beneficiaryID && m.IsActive);

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
                var beneficiaries = await this._edPayAppContext.Beneficiaries
                .Where(b => b.IsActive)
                .OrderBy(b => b.NickName)
                .ToListAsync();

                return beneficiaries;
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
        public async Task<IReadOnlyList<Beneficiary>> GetAllAsync(int userID)
        {
            try
            {
                // Requirement: The user should be able to view available top-up beneficiaries.
                var beneficiaries = await this._edPayAppContext.Beneficiaries
                .Where(b => b.IsActive && b.UserID == userID)
                .OrderBy(b => b.NickName)
                .ToListAsync();

                return beneficiaries;
            }
            catch (Exception)
            {
                throw;
            }
        } 

        /// <summary>
        /// Registers a new top-up beneficiary in system.
        /// </summary>
        /// <param name="beneficiary"></param>
        /// <returns>UserCreationEnum</returns>
        public async Task<UserCreationEnum> CreateAsync(Beneficiary beneficiary)
        {
            try
            {
                if (beneficiary == null) return UserCreationEnum.InvalidUserError;

                // Check if userID exists or not
                var isValidUserID = await this._userRepository.Exists(beneficiary.UserID);

                // If user does not exists, return invalid userID error
                if (!isValidUserID)
                    return UserCreationEnum.InvalidUserID;

                // Requirement: The user can add a maximum of 5 active top-up beneficiaries.
                // Check the total number of existing beneficiaries. If permitted, proceed to next set of data validations.
                var count = await this.GetActiveBeneficiariesCount(beneficiary.UserID);

                if (count >= Constants.BeneficiaryLimit)
                    return UserCreationEnum.BeneficiaryLimitExceeded;

                // Check if user exists by nickName
                var nickNameExists = await this._edPayAppContext.Beneficiaries
                    .FirstOrDefaultAsync(u => u.NickName.ToLower() == beneficiary.NickName.ToLower());

                // If exists, return duplicate nickName error
                if (nickNameExists != null)
                    return UserCreationEnum.UserNameExists;

                // Check if user exists by phone number
                var phoneExists = await this._edPayAppContext.Beneficiaries.FirstOrDefaultAsync(u => u.PhoneNumber == beneficiary.PhoneNumber);

                // If exists, return duplicate phoneNumber error
                if (phoneExists != null)
                    return UserCreationEnum.PhoneNumberExists;

                beneficiary.IsActive = true;
                await this._edPayAppContext.Beneficiaries.AddAsync(beneficiary);

                int rowsAffected = await this._edPayAppContext.SaveChangesAsync();

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
            try
            {
                if (entity == null) return -1;

                var beneficiary = await this.GetAsync(entity.ID);

                if (beneficiary == null)
                    return Constants.InvalidBeneficiary;

                beneficiary.NickName = entity.NickName;

                int rowsAffected = await this._edPayAppContext.SaveChangesAsync();

                return rowsAffected;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deactivates a beneficiary from system.
        /// </summary>
        /// <param name="beneficiaryID"></param> 
        /// <returns>Rows affected count</returns>
        public async Task<int> DeleteAsync(int beneficiaryID)
        {
            try
            {
                var beneficiary = await this.GetAsync(beneficiaryID);

                if (beneficiary == null)
                    return Constants.InvalidBeneficiary;

                beneficiary.IsActive = false;
                int rowsAffected = await this._edPayAppContext.SaveChangesAsync();

                return rowsAffected;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns count of all active beneficiaries for given userID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>Active count</returns>
        public async Task<int?> GetActiveBeneficiariesCount(int userID)
        {
            try
            {
                // Check if userID exists or not
                var isValidUserID = await this._userRepository.Exists(userID);

                // If user does not exists, return null.
                if (!isValidUserID)
                    return null;

                var count = await this._edPayAppContext.Beneficiaries.CountAsync(b => b.IsActive && b.UserID == userID);

                return count;
            }
            catch (Exception)
            {
                throw;
            }
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