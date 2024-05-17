using EdPay.Common.Domain.Core;
using System.ComponentModel.DataAnnotations;

namespace EdPay.UserApp.Domain.Models
{
    public class Beneficiary : Entity
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        [MaxLength(20)]
        public string NickName { get; set; }

        [Phone]
        [MaxLength(10)]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }
    }
}