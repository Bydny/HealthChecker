using HealthChecker.ServiceBus.Interfaces;
using HealthChecker.ServiceBus.Interfaces.BusControl;
using System;
using HealthChecker.ServiceBus.Config;

namespace HealthChecker.ServiceBus
{
    public class BusControlConfigurator
    {
        public const int RpcTimeoutDefault = 5000;
        public const long MessageTtlDefault = 1500;
        public const long QueueTtlDefault = 10000;
        private readonly RabbitOptions _rabbitOptions;

        private readonly Func<RabbitOptions, int, long, long, IBusControl> _busControlFactory;
        public IBusControl BusControl { get; private set; }


        public BusControlConfigurator(Func<RabbitOptions, int, long, long, IBusControl> busControlFactory)
        {
            _busControlFactory = busControlFactory ?? throw new ArgumentNullException(nameof(busControlFactory));
            _rabbitOptions = new RabbitOptions();
        }

        public BusControlConfigurator AddHost(string hostname)
        {
            _rabbitOptions.HostName = hostname ?? throw new ArgumentNullException(nameof(hostname));
            return this;
        }

        public BusControlConfigurator SetPassword(string password)
        {
            _rabbitOptions.Password= password ?? throw new ArgumentNullException(nameof(password));
            return this;
        }

        public BusControlConfigurator SetUsername(string username)
        {
            _rabbitOptions.UserName = username ?? throw new ArgumentNullException(nameof(username));
            return this;
        }

        public BusControlConfigurator AddConsumer<TConsumer, TRequest, TResponse>()
            where TConsumer : IConsumer<TRequest, TResponse>, new()
            where TRequest : IRequest
            where TResponse : IResponse
        {
            if (BusControl == null) 
                throw new InvalidOperationException($"{nameof(IBusControl)} is not built. Call {nameof(AddHost)}() before consumer adding");
            
            BusControl.AddConsumer<TConsumer, TRequest, TResponse>(new TConsumer());
            return this;
        }

        public void BuildBusControl()
        {
            BusControl = _busControlFactory(_rabbitOptions, RpcTimeoutDefault, MessageTtlDefault, QueueTtlDefault);
        }

    }
}
