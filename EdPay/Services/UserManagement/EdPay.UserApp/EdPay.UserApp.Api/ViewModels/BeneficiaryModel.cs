namespace EdPay.UserApp.Api.ViewModels
{
    /// <summary>
    /// User Beneficiary view model
    /// </summary>
    public class BeneficiaryModel
    {
        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Beneficiary nick name is required")]
        [MaxLength(20)]
        public string NickName { get; set; }

        [Phone]
        [Required(ErrorMessage = "PhoneNumber is required")]
        [MinLength(10)]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
    }
}