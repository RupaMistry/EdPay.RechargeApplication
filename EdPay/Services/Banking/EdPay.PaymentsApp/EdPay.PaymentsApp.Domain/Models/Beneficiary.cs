using EdPay.Common.Domain.Core;

namespace EdPay.PaymentsApp.Domain.Models
{
    /// <summary>
    /// Beneficiary entity
    /// </summary>
    public class Beneficiary : Entity
    {
        public int UserID { get; set; }

        public int BeneficiaryID { get; set; } 
    }
}