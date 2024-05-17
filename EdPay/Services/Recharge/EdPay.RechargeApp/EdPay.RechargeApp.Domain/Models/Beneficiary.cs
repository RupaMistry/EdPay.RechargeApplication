using EdPay.Common.Domain.Core;

namespace EdPay.RechargeApp.Domain.Models
{
    /// <summary>
    /// Beneficiary entity.
    /// </summary>
    public class Beneficiary : Entity
    {
        public int UserID { get; set; }

        public int BeneficiaryID { get; set; } 

        public string PhoneNumber { get; set; }
    }
}