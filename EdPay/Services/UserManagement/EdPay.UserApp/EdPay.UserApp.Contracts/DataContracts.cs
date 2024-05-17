namespace EdPay.UserApp.Contracts
{
    // Recharge Message Contracts
    public record RechargeUserCreated(int UserID, bool IsVerified);

    public record RechargeUserDeleted(int UserID);

    public record RechargeBeneficiaryCreated(int UserID, int BeneficiaryID, string PhoneNumber);

    public record RechargeBeneficiaryDeleted(int BeneficiaryID);

    // Payment Message Contracts

    public record PaymentUserCreated(int UserID, bool IsVerified);

    public record PaymentUserDeleted(int UserID);

    public record PaymentBeneficiaryCreated(int UserID, int BeneficiaryID);

    public record PaymentBeneficiaryDeleted(int BeneficiaryID);
}