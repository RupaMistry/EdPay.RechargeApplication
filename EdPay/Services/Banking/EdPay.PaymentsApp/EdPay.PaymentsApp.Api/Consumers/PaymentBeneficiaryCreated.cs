namespace EdPay.PaymentsApp.Api.Consumers
{
    /// <summary>
    /// MassTransit Payment Consumer for new Beneficiary creation[UserAppDB]
    /// </summary>
    public class PaymentBeneficiaryCreatedConsumer(IRepository<Beneficiary> repository) : IConsumer<PaymentBeneficiaryCreated>
    {
        private readonly IRepository<Beneficiary> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task Consume(ConsumeContext<PaymentBeneficiaryCreated> context)
        {
            var message = context.Message;

            var beneficiary = await _repository.GetAsync(message.BeneficiaryID);

            // if beneficiary already exist, then don't create again.
            if (beneficiary != null)
                return;

            beneficiary = new Beneficiary
            {
                UserID = message.UserID,
                BeneficiaryID = message.BeneficiaryID
            };

            await repository.CreateAsync(beneficiary);
        }
    } 
}