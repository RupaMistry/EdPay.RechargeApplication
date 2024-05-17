namespace EdPay.PaymentsApp.Domain.Models
{
    public record UserBankingDetails(int UserID, decimal AvailableBalance);

    public record DebitTransactionRequest(int UserID, int BeneficiaryID, string AmountCurrency, decimal Amount, decimal ServiceFee, string TransactionType);

    public record DebitTransactionResponse(TransactionStatusEnum transactionStatus, int UserID, int TransactionID, bool IsSuccess, string ErrorMessage);

    public record CreditTransactionRequest(int UserID, string AmountCurrency, decimal Amount, decimal ServiceFee, string TransactionType);

    public record CreditTransactionResponse(TransactionStatusEnum TransactionStatus, int UserID, int TransactionID, bool IsSuccess, string ErrorMessage);
}