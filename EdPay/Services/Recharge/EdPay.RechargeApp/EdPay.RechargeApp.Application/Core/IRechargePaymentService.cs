using EdPay.RechargeApp.Domain;

namespace EdPay.RechargeApp.Application.Core
{
    /// <summary>
    /// IService for User entity
    /// </summary>
    public interface IRechargePaymentService<T> where T : Entity
    {
        /// <summary>
        /// Validates RechargePayment request.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns><![CDATA[Task<RechargeValidationEnum>]]></returns>
        Task<RechargeValidationEnum> ValidateRechargeRequest(RechargePayment payment);

        /// <summary>
        /// Inserts a new RechargeHistory record.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        Task<int> InsertRechargeHistory(RechargePayment rechargePayment);
    } 
}