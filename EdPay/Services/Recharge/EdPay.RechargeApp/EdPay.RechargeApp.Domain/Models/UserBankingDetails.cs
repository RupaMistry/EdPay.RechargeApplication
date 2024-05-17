namespace EdPay.RechargeApp.Domain.Models
{ 
    public record UserBankingDetails(int UserID, decimal AvailableBalance);
     
    public record DebitTransactionResponse(int UserID, int TransactionID , bool IsSucess, string ErrorMessage);
}