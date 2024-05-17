namespace EdPay.PaymentsApp.Domain
{
    /// <summary>
    /// Represents Recharge validation status enum
    /// </summary>
    public enum TransactionStatusEnum
    { 
        InvalidUser = 1,
        InvalidBeneficiary = 2, 
        Success = 3,
        Failure = 4
    }
}