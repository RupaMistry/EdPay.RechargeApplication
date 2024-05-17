namespace EdPay.RechargeApp.Api.Consumers
{
    /// <summary>
    /// MassTransit Recharge Consumer for new User deletion[UserAppDB]
    /// </summary>
    public class RechargeUserDeletedConsumer(IRepository<User> repository) : IConsumer<RechargeUserDeleted>
    {
        private readonly IRepository<User> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task Consume(ConsumeContext<RechargeUserDeleted> context)
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
