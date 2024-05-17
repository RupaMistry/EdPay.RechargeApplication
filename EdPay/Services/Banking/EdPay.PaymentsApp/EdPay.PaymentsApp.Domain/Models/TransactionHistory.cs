using EdPay.Common.Domain.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EdPay.PaymentsApp.Domain.Models
{
    /// <summary>
    /// TransactionHistory entity
    /// </summary>
    public class TransactionHistory : Entity
    { 
        [Required] 
        public int UserID { get; set; }
         
        [Required] 
        public int BeneficiaryID { get; private set; }

        [Required] 
        public bool IsCredit { get; private set; } 

        [Required]
        [MaxLength(3)]
        public string AmoutCurrency { get; private set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal OpeningBalance { get; private set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ClosingBalance { get; private set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; private set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ServiceFee { get; private set; }

        [Required]
        public int CurrentMonth { get; private set; }

        [Required]
        public DateTime TransactionDate { get; private set; }

        [Required]
        public bool IsSuccess { get; private set; }

        public string TransactionType { get; private set; }

        public string ErrorMessage { get; private set; }
    }
}