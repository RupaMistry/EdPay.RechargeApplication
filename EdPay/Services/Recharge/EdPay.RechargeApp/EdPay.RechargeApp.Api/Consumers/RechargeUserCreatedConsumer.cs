namespace EdPay.RechargeApp.Api.Consumers
{
    /// <summary>
    /// MassTransit Recharge Consumer for new User creation[UserAppDB]
    /// </summary>
    public class RechargeUserCreatedConsumer(IRepository<User> repository) : IConsumer<RechargeUserCreated>
    {
        private readonly IRepository<User> _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        public async Task Consume(ConsumeContext<RechargeUserCreated> context)
        {
            var message = context.Message;

            var user = await _repository.GetAsync(message.UserID);

            // if user already exist, then don't create again.
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