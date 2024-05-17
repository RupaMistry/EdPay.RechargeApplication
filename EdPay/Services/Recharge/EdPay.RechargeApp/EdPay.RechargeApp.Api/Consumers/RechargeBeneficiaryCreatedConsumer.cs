namespace EdPay.RechargeApp.Api.Consumers
{
    /// <summary>
    /// MassTransit Recharge Consumer for new Beneficiary creation[UserAppDB]
    /// </summary>
    public class RechargeBeneficiaryCreatedConsumer(IRepository<Beneficiary> repository) : IConsumer<RechargeBeneficiaryCreated>
    {
        private readonly IRepository<Beneficiary> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        public async Task Consume(ConsumeContext<RechargeBeneficiaryCreated> context)
        {
            var message = context.Message;

            var beneficiary = await _repository.GetAsync(message.BeneficiaryID);

            // if beneficiary already exist, then dont create again.
            if (beneficiary != null) 
                return;

           beneficiary = new Beneficiary
            {
                UserID = message.UserID,
                BeneficiaryID = message.BeneficiaryID,
                PhoneNumber = message.PhoneNumber
            };

            await repository.CreateAsync(beneficiary);
        }
    }
}