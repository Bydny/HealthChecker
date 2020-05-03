using HealthChecker.ServiceBus.Interfaces.BusControl;
using Microsoft.Extensions.ObjectPool;
using System;

namespace HealthChecker.ServiceBus
{
    public class RabbitMqBusControlFactory : IBusControlFactory
    {
        public IBusControl Create(Action<BusControlConfigurator> configure)
        {
            var busConfigurator = new BusControlConfigurator(
                (options, rpcTimeout, messageTtl, queueTtl) =>
                {
                    var provider = new DefaultObjectPoolProvider();
                    var channelPool = provider.Create(new RabbitModelPooledObjectPolicy(options));
                    return new RabbitMqBusControl(channelPool, rpcTimeout, messageTtl, queueTtl);
                });
            configure.Invoke(busConfigurator);
            return busConfigurator.BusControl;
        }
    }
}
