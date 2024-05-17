using EdPay.Common.Domain;
using EdPay.Common.Domain.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace EdPay.RechargeApp.Domain.Models
{ 
    /// <summary>
    /// RechargePayment entity.
    /// </summary>
    public class RechargePayment : Entity
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public int BeneficiaryID { get;  set; }
         
        public string AmoutCurrency { get => Currency.UAE; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get;  set; } 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ServiceFee { get => RechargeFees.ServiceFee; }

        [IgnoreDataMember]
        public string TransactionType { get => "Recharge"; }
    }
}