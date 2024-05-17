using EdPay.Common.Domain.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdPay.RechargeApp.Domain.Models
{
    /// <summary>
    /// CreditRule entity.
    /// </summary>
    public class CreditRule : Entity
    {
        [MaxLength(100)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal AmountLimit { get; set; }

        public bool PerBeneficiary { get; set; }
    }
}