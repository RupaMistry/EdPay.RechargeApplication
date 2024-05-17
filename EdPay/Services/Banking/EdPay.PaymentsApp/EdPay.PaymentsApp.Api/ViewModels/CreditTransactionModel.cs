using EdPay.Common.Domain;
using System.ComponentModel.DataAnnotations;

namespace EdPay.PaymentsApp.Api.ViewModels
{
    /// <summary>
    /// CreditTransaction view model.
    /// </summary>
    public class CreditTransactionModel
    {
        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get; set; } 

        [Required(ErrorMessage = "Recharge Amount is required")]
        public decimal Amount { get;  set; }

        [Required(ErrorMessage = "ServiceFee is required")]
        public decimal ServiceFee { get;  set; }

        public string TransactionType { get => "CreditMoney"; } 

        public string AmoutCurrency { get => Currency.UAE; }
    }
}