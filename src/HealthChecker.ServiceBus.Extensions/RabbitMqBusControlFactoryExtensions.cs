using HealthChecker.ServiceBus.Interfaces.BusControl;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HealthChecker.ServiceBus.Extensions
{
    public static class RabbitMqBusControlFactoryExtensions
    {
        public static void AddRabbitMq(this IServiceCollection serviceCollection, Action<BusControlConfigurator> configure)
        {
            serviceCollection.AddSingleton<IBusControl>(sp =>
            {
                var factory = new RabbitMqBusControlFactory();
                var busControl = factory.Create(configure);
                return busControl;
            });
        }
    }
}
