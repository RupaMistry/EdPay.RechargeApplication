namespace EdPay.RechargeApp.Api.Consumers
{
    /// <summary>
    /// MassTransit Recharge Consumer for new Beneficiary deletion[UserAppDB]
    /// </summary>
    public class RechargeBeneficiaryDeletedConsumer(IRepository<Beneficiary> repository) : IConsumer<RechargeBeneficiaryDeleted>
    {
        private readonly IRepository<Beneficiary> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task Consume(ConsumeContext<RechargeBeneficiaryDeleted> context)
        {
            var message = context.Message;

            var beneficiary = await _repository.GetAsync(message.BeneficiaryID);

            // if beneficiary not found, then return. No record to delete.
            if (beneficiary == null)
                return;

            await repository.DeleteAsync(message.BeneficiaryID);
        }
    }
}