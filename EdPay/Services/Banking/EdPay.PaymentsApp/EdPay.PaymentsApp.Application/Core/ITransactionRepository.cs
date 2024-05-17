namespace EdPay.PaymentsApp.Application.Core
{
    /// <summary>
    /// IRepository for PaymentTransaction entity.
    /// </summary>
    public interface ITransactionRepository<T> where T : Entity
    {
        /// <summary>
        /// Performs a credit transaction on User bank account system.
        /// </summary>
        /// <param name="creditTransactionRequest">The credit transaction request.</param>
        /// <returns>A CreditTransactionResponse</returns>
        CreditTransactionResponse CreditAmountAsync(CreditTransactionRequest creditTransactionRequest);

        /// <summary>
        /// Performs a debit transaction on User bank account system.
        /// </summary>
        /// <param name="debitTransactionRequest">The debit transaction request.</param>
        /// <returns><![CDATA[Task<DebitTransactionResponse>]]></returns>
        Task<DebitTransactionResponse> DebitAmountAsync(DebitTransactionRequest debitTransactionRequest);
    }
}