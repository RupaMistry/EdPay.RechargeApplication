using EdPay.Common.Domain;

namespace EdPay.RechargeApp.Api.ViewModels
{
    /// <summary>
    /// RechargePayment view model.
    /// </summary>
    public class RechargePaymentModel
    {
        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "BeneficiaryID is required")]
        public int BeneficiaryID { get;  set; } 

        public string AmoutCurrency { get => Currency.UAE; }

        [Required(ErrorMessage = "Recharge Amount is required")]
        [Range(5d, 100d)] // Recharge amount should be in range of 5 to 100 as per Topup options
        public decimal Amount { get;  set; } 
    }
}