using EdPay.PaymentsApp.Api.ViewModels;

namespace EdPay.PaymentsApp.Api.ExtensionMethods
{
    /// <summary>
    /// Extension methods for Api.ViewModels.
    /// </summary>
    public static class ViewModelMapperExtensions
    {
        /// <summary>
        /// Maps and return DebitTransactionRequest.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>DebitTransactionRequest</returns>
        public static DebitTransactionRequest AsEntity(this DebitTransactionModel viewModel)
        {
            return new DebitTransactionRequest(viewModel.UserID,
                viewModel.BeneficiaryID, viewModel.AmoutCurrency, viewModel.Amount, viewModel.ServiceFee,
                viewModel.TransactionType);
        }

        /// <summary>
        /// Maps and return CreditTransactionRequest.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>CreditTransactionRequest</returns>
        public static CreditTransactionRequest AsEntity(this CreditTransactionModel viewModel)
        {
            return new CreditTransactionRequest(viewModel.UserID, viewModel.AmoutCurrency, viewModel.Amount,
                viewModel.ServiceFee, viewModel.TransactionType);
        }

        /// <summary>
        /// Maps and return UserBankAccount.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>UserBankAccount</returns>
        public static UserBankAccount AsEntity(this UserBankAccountModel viewModel)
        {
            return new UserBankAccount()
            {
                UserID = viewModel.UserID,
                AvailableBalance = viewModel.AvailableBalance,
                EdPayCardNumber = viewModel.EdPayCardNumber,
                CompanyName = viewModel.CompanyName,
                IsAccountActive = viewModel.IsAccountActive
            };
        }
    }
}