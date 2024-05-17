using EdPay.Common.Domain;
using System.ComponentModel.DataAnnotations;

namespace EdPay.PaymentsApp.Api.ViewModels
{
    /// <summary>
    /// DebitTransaction view model.
    /// </summary>
    public class DebitTransactionModel
    {
        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "BeneficiaryID is required")]
        public int BeneficiaryID { get;  set; } 

        [Required(ErrorMessage = "Recharge Amount is required")]
        [Range(5d, 100d)] // Recharge amount should be in range of 5 to 100 as per Topup options
        public decimal Amount { get;  set; }

        [Required(ErrorMessage = "ServiceFee is required")]
        public decimal ServiceFee { get;  set; }

        public string TransactionType { get;  set; } 

        public string AmoutCurrency { get => Currency.UAE; }
    }
}