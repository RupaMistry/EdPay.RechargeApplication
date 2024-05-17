namespace EdPay.PaymentsApp.Api.Consumers
{
    /// <summary>
    /// MassTransit Payment Consumer for Beneficiary deletion[UserAppDB]
    /// </summary>
    public class PaymentBeneficiaryDeletedConsumer(IRepository<Beneficiary> repository) : IConsumer<PaymentBeneficiaryDeleted>
    {
        private readonly IRepository<Beneficiary> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task Consume(ConsumeContext<PaymentBeneficiaryDeleted> context)
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
