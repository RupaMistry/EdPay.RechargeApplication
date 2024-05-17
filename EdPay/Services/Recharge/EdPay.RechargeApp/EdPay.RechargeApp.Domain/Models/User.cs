using EdPay.Common.Domain.Core;

namespace EdPay.RechargeApp.Domain.Models
{
    /// <summary>
    /// User entity.
    /// </summary>
    public class User : Entity
    { 
        public int UserID { get; set; }

        public bool IsVerified { get; set; } 
    }
}