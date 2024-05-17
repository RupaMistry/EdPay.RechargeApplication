using EdPay.Common.Domain.Core;
using System.ComponentModel.DataAnnotations; 

namespace EdPay.PaymentsApp.Domain.Models
{
    /// <summary>
    /// TransactionPurpose entity
    /// </summary>
    public class TransactionPurpose : Entity
    {
        [MaxLength(100)]
        public string Description { get; set; }
    }
}