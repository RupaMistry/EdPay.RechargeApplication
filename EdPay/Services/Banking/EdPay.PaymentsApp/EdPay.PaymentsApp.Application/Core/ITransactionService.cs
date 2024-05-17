namespace EdPay.PaymentsApp.Application.Core
{
    public interface ITransactionService<in T> where T : Entity
    {
        /// <summary>
        /// Performs a credit transaction on User bank account system.
        /// </summary>
        /// <param name="creditTransactionRequest">The credit transaction request.</param>
        /// <returns>A CreditTransactionResponse</returns>
        Task<CreditTransactionResponse> CreditAmountAsync(CreditTransactionRequest creditTransactionRequest);

        /// <summary>
        /// Performs a debit transaction on User bank account system.
        /// </summary>
        /// <param name="debitTransactionRequest">The debit transaction request.</param>
        /// <returns><![CDATA[Task<DebitTransactionResponse>]]></returns>
        Task<DebitTransactionResponse> DebitAmountAsync(DebitTransactionRequest debitTransactionRequest); 
    }
}