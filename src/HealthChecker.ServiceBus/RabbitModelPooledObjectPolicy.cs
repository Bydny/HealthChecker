using HealthChecker.ServiceBus.Config;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;

namespace HealthChecker.ServiceBus
{
    public class RabbitModelPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitOptions _options;

        private readonly IConnection _connection;

        public RabbitModelPooledObjectPolicy(RabbitOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName ?? throw new ArgumentNullException(nameof(_options.HostName)),
                UserName = _options.UserName ?? throw new ArgumentNullException(nameof(_options.UserName)),
                Password = _options.Password ?? throw new ArgumentNullException(nameof(_options.Password)),
            };

            return factory.CreateConnection();
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
