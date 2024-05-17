using EdPay.Common.Domain.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdPay.RechargeApp.Domain.Models
{
    /// <summary>
    /// RechargeHistory entity.
    /// </summary>
    public class RechargeHistory: Entity
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public int BeneficiaryID { get;  set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get;  set; }
    }
}