using EdPay.Common.Domain.Core;
using System.ComponentModel.DataAnnotations;

namespace EdPay.UserApp.Domain.Models
{
    public class User : Entity
    {
        [Required]
        [MaxLength(30)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        public bool IsVerified { get; set; }

        [Phone]
        [MinLength(10)]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [MaxLength(50)]
        public string EmailAddress { get; set; }

        [MaxLength(50)]
        public string Nationality { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}