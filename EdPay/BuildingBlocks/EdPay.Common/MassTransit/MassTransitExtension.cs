using EdPay.Common.Configuration;
using EdPay.Common.Domain;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EdPay.Common.MassTransit
{
    /// <summary>
    /// Common MassTransit extension class.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serviceSettings"></param>
    /// <param name="rabbitMQSettings"></param>
    public static class MassTransitExtension
    {
        /// <summary>
        ///  Registers and configures MassTransit library with RabbitMq service.
        /// </summary>
        public static IServiceCollection AddMassTransitWithRabbitMq(
            this IServiceCollection services, ServiceSettings serviceSettings,
            RabbitMQSettings rabbitMQSettings)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());

                configure.UsingRabbitMq((context, configurator) =>
                {
                    var configuration = context.GetService<IConfiguration>();

                    configurator.Host(rabbitMQSettings.Host);

                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

                    configurator.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(MassTransitConstants.RetryCount, TimeSpan.FromSeconds(MassTransitConstants.RetryTimeInSeconds));
                    });
                });
            });

            return services;
        }
    }
}