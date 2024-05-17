namespace EdPay.PaymentsApp.Api.Consumers
{
    /// <summary>
    /// MassTransit Payment Consumer for User deletion[UserAppDB]
    /// </summary>
    public class PaymentUserDeletedConsumer(IRepository<User> repository) : IConsumer<PaymentUserDeleted>
    {
        private readonly IRepository<User> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task Consume(ConsumeContext<PaymentUserDeleted> context)
        {
            var message = context.Message;

            var user = await _repository.GetAsync(message.UserID);

            // if user not found, then return. No record to delete.
            if (user == null)
                return;

            await _repository.DeleteAsync(message.UserID);
        }
    }
}
