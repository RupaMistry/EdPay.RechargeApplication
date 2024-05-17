using EdPay.Common.Domain.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdPay.PaymentsApp.Domain.Models
{
    /// <summary>
    /// UserBankAccount entity.
    /// </summary>
    public class UserBankAccount : Entity
    {
        [Required]
        public int UserID { get;  set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal AvailableBalance { get;  set; } 

        [Required]
        public string EdPayCardNumber { get;  set; }

        [Required]
        public string CompanyName { get;  set; }

        [Required]
        public bool IsAccountActive { get;  set; }

        [Required]
        public DateTime RegistrationDate { get; set; } 
    } 
}