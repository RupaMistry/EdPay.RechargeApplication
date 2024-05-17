using System.ComponentModel.DataAnnotations;

namespace EdPay.PaymentsApp.Api.ViewModels
{
    /// <summary>
    /// UserBankAccount view model.
    /// </summary>
    public class UserBankAccountModel
    {
        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get;  set; }

        [Required(ErrorMessage = "AvailableBalance is required")]
        public decimal AvailableBalance { get; set; }

        [Required(ErrorMessage = "EdPayCardNumber is required")]
        public string EdPayCardNumber { get;  set; }

        [Required(ErrorMessage = "CompanyName is required")]
        public string CompanyName { get;  set; }

        [Required(ErrorMessage = "IsAccountActive is required")]
        public bool IsAccountActive { get;  set; } 
    }
}