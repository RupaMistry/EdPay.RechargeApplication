namespace EdPay.RechargeApp.Domain
{
    /// <summary>
    /// Represents Recharge validation status enum
    /// </summary>
    public enum RechargeValidationEnum
    {
        InvalidTopupPlan = 1,
        InvalidUser = 2,
        InvalidBeneficiary = 3,
        NoBeneficiaryMappingFound = 4,
        MonthlyRechargeLimitExceeded = 5,
        RechargeLimitExceeded = 6,
        Success = 7,
        ValidationError=8
    }

    /// <summary>
    /// Represents Recharge credit rules enum
    /// </summary>
    public enum CreditRulesEnum
    {
        NonVerifiedUser = 1000,
        VerifiedUser = 500,
        VerifiedUserPerMonth = 3000
    }

    /// <summary>
    /// Represents Recharge Fees constants
    /// </summary>
    public static class RechargeFees
    {
        // Requirement:	A charge of AED 1 should be applied for every top-up transaction.
        public const decimal ServiceFee = 1;

        public const decimal PlatformFee = 2;

        public const decimal VATFee = 5;
    }

    /// <summary>
    /// Constants for storing third party ServiceAPI action url names.
    /// </summary>
    public static class ServiceClientApiURL
    { 
        public const string Payment_GetBalance = "api/payments/balance";
        public const string Payment_DebitTransaction = "api/payments/debit";
        public const string Payment_CreditTransaction = "api/payments/credit";
    }
}