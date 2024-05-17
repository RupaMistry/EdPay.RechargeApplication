namespace EdPay.PaymentsApp.Api.Consumers
{
    /// <summary>
    /// MassTransit Payment Consumer for User creation[UserAppDB]
    /// </summary>
    public class PaymentUserCreatedConsumer(IRepository<User> repository) : IConsumer<PaymentUserCreated>
    {
        private readonly IRepository<User> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        public async Task Consume(ConsumeContext<PaymentUserCreated> context)
        {
            var message = context.Message;

            var user = await _repository.GetAsync(message.UserID);

            // if user already exist, then dont create again.
            if (user != null)
                return;

            user = new User
            {
                UserID = message.UserID,
                IsVerified = message.IsVerified
            };

            await repository.CreateAsync(user);
        }
    }
}