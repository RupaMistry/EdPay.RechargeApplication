namespace EdPay.RechargeApp.Api.ExtensionMethods
{
    /// <summary>
    /// Extension methods for Api.ViewModels.
    /// </summary>
    public static class ViewModelMapperExtensions
    {
        /// <summary>
        /// Maps and return RechargePayment.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <returns>RechargePayment</returns>
        public static RechargePayment AsEntity(this RechargePaymentModel viewModel)
        {
            return new RechargePayment()
            {
                UserID = viewModel.UserID,
                BeneficiaryID = viewModel.BeneficiaryID,
                Amount = viewModel.Amount
            };
        } 
    }
}