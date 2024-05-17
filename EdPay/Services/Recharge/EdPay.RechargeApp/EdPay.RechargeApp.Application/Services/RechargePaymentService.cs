using EdPay.RechargeApp.Domain;

namespace EdPay.RechargeApp.Application.Services
{
    /// <summary>
    /// RechargePayment service.
    /// </summary>
    /// <param name="topupPlanRepository">The topup plan repository.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="beneficiaryRepository">The beneficiary repository.</param>
    /// <param name="rechargeHistoryRepository">The recharge history repository.</param>
    public class RechargePaymentService(
        ITopupPlanRepository<TopupPlan> topupPlanRepository,
        IRepository<User> userRepository,
        IRepository<Beneficiary> beneficiaryRepository,
        IRechargeHistoryRepository<RechargeHistory> rechargeHistoryRepository)
        : IRechargePaymentService<RechargePayment>
    {
        private readonly ITopupPlanRepository<TopupPlan> _topupPlanRepository =
            topupPlanRepository ?? throw new ArgumentNullException(nameof(topupPlanRepository));

        private readonly IRepository<User> _userRepository =
            userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        private readonly IRepository<Beneficiary> _beneficiaryRepository =
            beneficiaryRepository ?? throw new ArgumentNullException(nameof(beneficiaryRepository));

        private readonly IRechargeHistoryRepository<RechargeHistory> _rechargeHistoryRepository =
            rechargeHistoryRepository ?? throw new ArgumentNullException(nameof(rechargeHistoryRepository));

        /// <summary>
        /// Inserts a new RechargeHistory record.
        /// </summary>
        /// <param name="rechargePayment">The recharge payment.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        public async Task<int> InsertRechargeHistory(RechargePayment rechargePayment)
        {
            return await _rechargeHistoryRepository.InsertRechargeHistory(
                new RechargeHistory()
                {
                    UserID = rechargePayment.UserID,
                    Amount = rechargePayment.Amount,
                    BeneficiaryID = rechargePayment.BeneficiaryID,
                    TransactionDate = DateTime.UtcNow
                });
        }

        /// <summary>
        /// Validates RechargePayment request.
        /// </summary>
        /// <param name="rechargePayment">The recharge payment.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns><![CDATA[Task<RechargeValidationEnum>]]></returns>
        public async Task<RechargeValidationEnum> ValidateRechargeRequest(RechargePayment rechargePayment)
        {
            try
            {
                if (rechargePayment == null) return RechargeValidationEnum.ValidationError;

                // 1. Check topup amount is in TopupOptions list
                var planExists = await this._topupPlanRepository.Exists(rechargePayment.Amount);

                if (!planExists)
                    return RechargeValidationEnum.InvalidTopupPlan;

                // 2. Check if user exists or not
                var userExists = await this._userRepository.Exists(rechargePayment.UserID);

                if (!userExists)
                    return RechargeValidationEnum.InvalidUser;

                // 3. Check if beneficiary is active or not. If active, check if beneficiary is mapped for the given userID
                var beneficiary = await this._beneficiaryRepository.GetAsync(rechargePayment.BeneficiaryID);
                 
                if (beneficiary?.ID <= 0)
                    return RechargeValidationEnum.InvalidBeneficiary;

                if (beneficiary != null && beneficiary.UserID != rechargePayment.UserID)
                    return RechargeValidationEnum.NoBeneficiaryMappingFound;

                // 4. Check if recharge amount is within the CreditAvailable(RechargeHistory)
                var hasRechargeExceeded =
                    await this._rechargeHistoryRepository.HasUserRechargeLimitExceeded(rechargePayment.UserID);

                // IF exceeded, return validation error.
                if (hasRechargeExceeded)
                    return RechargeValidationEnum.MonthlyRechargeLimitExceeded;

                // 5. Get user balance amount based on IsVerified status.  
                var availableRechargeCredit =
                    await this._rechargeHistoryRepository.GetAvailableRechargeCredit(rechargePayment);

                // If user tries to recharge with amount greater than he has recharge limit, return.
                if (rechargePayment.Amount > availableRechargeCredit)
                    return RechargeValidationEnum.RechargeLimitExceeded;

                return RechargeValidationEnum.Success;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}