namespace EdPay.UserApp.Api.ViewModels
{
    /// <summary>
    /// User view model
    /// </summary>
    public class UserModel
    {
        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(30)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(100)]
        public string FullName { get; set; }

        public bool IsVerified { get; set; }  

        [EmailAddress]
        [MaxLength(50)]
        [Required(ErrorMessage = "EmailAddress is required")]
        public string EmailAddress { get; set; }

        [Phone]
        [MinLength(10)]
        [MaxLength(10)]
        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; }

        [MaxLength(50)]
        public string Nationality { get; set; } 
    }
}