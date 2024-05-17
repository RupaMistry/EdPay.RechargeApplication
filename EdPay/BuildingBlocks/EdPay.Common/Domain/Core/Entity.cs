using System.ComponentModel.DataAnnotations;

namespace EdPay.Common.Domain.Core
{
    /// <summary>
    /// Base class for all DB entities.
    /// </summary>
    public class Entity
    {
        [Key] 
        public int ID { get; set; }
    }
}