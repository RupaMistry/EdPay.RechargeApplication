using EdPay.PaymentsApp.Application.Core;

namespace EdPay.PaymentsApp.Application.Services
{
    /// <summary>
    /// PaymentTransaction service.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="beneficiaryRepository">The beneficiary repository.</param>
    /// <param name="transactionRepository">The transaction repository.</param>
    public class TransactionService(
        IRepository<User> userRepository,
        IRepository<Beneficiary> beneficiaryRepository,
        ITransactionRepository<TransactionHistory> transactionRepository) : ITransactionService<TransactionHistory>
    {
        private readonly IRepository<User> _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IRepository<Beneficiary> _beneficiaryRepository = beneficiaryRepository ?? throw new ArgumentNullException(nameof(beneficiaryRepository)); 
        private readonly ITransactionRepository<TransactionHistory> _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));

        /// <summary>
        /// Performs a debit transaction on User bank account system.
        /// </summary>
        /// <param name="debitTransactionRequest">The debit transaction request.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns><![CDATA[Task<DebitTransactionResponse>]]></returns>
        public async Task<DebitTransactionResponse> DebitAmountAsync(DebitTransactionRequest debitTransactionRequest)
        {
            ArgumentNullException.ThrowIfNull(nameof(debitTransactionRequest));

            var transactionStatus = TransactionStatusEnum.Success;

            // If user and beneficiary records exist as part of MessageExchange sync with MassTransit, proceed to debit().
            var userExists = await this._userRepository.Exists(debitTransactionRequest.UserID);

            if (!userExists)
                transactionStatus = TransactionStatusEnum.InvalidUser;

            var beneficiaryExists = await this._beneficiaryRepository.Exists(debitTransactionRequest.BeneficiaryID);

            if (!beneficiaryExists)
                transactionStatus = TransactionStatusEnum.InvalidBeneficiary;

            if (transactionStatus != TransactionStatusEnum.Success)
                return new DebitTransactionResponse(transactionStatus, 0, 0, false, string.Empty);

            // Perform debit transaction
            var debitResponse = await this._transactionRepository.DebitAmountAsync(debitTransactionRequest);

            return debitResponse;
        }

        /// <summary>
        /// Performs a credit transaction on User bank account system.
        /// </summary>
        /// <param name="creditTransactionRequest">The credit transaction request.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns><![CDATA[Task<CreditTransactionResponse>]]></returns>
        public async Task<CreditTransactionResponse> CreditAmountAsync(CreditTransactionRequest creditTransactionRequest)
        {
            ArgumentNullException.ThrowIfNull(nameof(creditTransactionRequest));

            var transactionStatus = TransactionStatusEnum.Success;

            // If user and beneficiary records exist as part of MessageExchange sync with MassTransit, proceed to debit().

            var userExists = await this._userRepository.Exists(creditTransactionRequest.UserID);

            if (!userExists)
                transactionStatus = TransactionStatusEnum.InvalidUser;
             
            if (transactionStatus != TransactionStatusEnum.Success)
                return new CreditTransactionResponse(transactionStatus, 0, 0, false, string.Empty);

            // Perform credit transaction
            var creditResponse =  this._transactionRepository.CreditAmountAsync(creditTransactionRequest);

            return creditResponse;
        } 
    }
}
