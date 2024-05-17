using EdPay.Common.Domain.Core;

namespace EdPay.RechargeApp.Domain.Models
{
    /// <summary>
    /// TopupPlan entity.
    /// </summary>
    public class TopupPlan : Entity
    {
        public int Amount { get; set; }

        public string Currency { get; set; }

        public string PlanDescription { get; set; }

        public bool IsActive { get; set; } 
    }
}