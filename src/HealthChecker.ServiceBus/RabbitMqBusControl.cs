using HealthChecker.ServiceBus.Interfaces;
using HealthChecker.ServiceBus.Interfaces.BusControl;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace HealthChecker.ServiceBus
{
    public class RabbitMqBusControl : IBusControl
    {
        private readonly ObjectPool<IModel> _channelPool;
        private readonly int _rpcTimeout;
        private readonly long _messageTtl;
        private readonly long _replyQueueTtl;

        public const string MessageTtlKey = "x-message-ttl";
        public const string QueueTtlKey = "x-expires";


        public RabbitMqBusControl(ObjectPool<IModel> channelPool, int rpcTimeout, long messageTtl, long queueTtl)
        {
            _channelPool = channelPool;
            _rpcTimeout = rpcTimeout;
            _messageTtl = messageTtl;
            _replyQueueTtl = queueTtl;
        }

        public TResponse Send<TRequest, TResponse>(TRequest message) 
            where TRequest : IRequest 
            where TResponse : IResponse
        {
            var channel = _channelPool.Get();

            try
            {
                var args = new Dictionary<string, object>
                {
                    {QueueTtlKey, _replyQueueTtl},
                    {MessageTtlKey, _messageTtl},
                };
                var replyQueueName = channel.QueueDeclare(arguments: args).QueueName;
                var replyConsumer = new EventingBasicConsumer(channel);
                var respQueue = new BlockingCollection<TResponse>();

                var publishProps = channel.CreateBasicProperties();
                var correlationId = Guid.NewGuid().ToString();
                publishProps.CorrelationId = correlationId;
                publishProps.ReplyTo = replyQueueName;

                replyConsumer.Received += (model, ea) =>
                {
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        var response = Deserialize<TResponse>(ea.Body.ToArray());
                        respQueue.Add(response);
                    }
                };

                var queue = typeof(TRequest).Name;
                QueueDeclare<TRequest>(channel);

                var body = Serialize(message);

                channel.BasicPublish(exchange: "",
                    routingKey: queue,
                    basicProperties: publishProps,
                    body: body);

                var replyConsumerTag = channel.BasicConsume(
                    consumer: replyConsumer,
                    queue: replyQueueName,
                    autoAck: true);

                var cancellationTokenSource = new CancellationTokenSource(_rpcTimeout);

                try
                {
                    return respQueue.Take(cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return default;
                }
                finally
                {
                    channel.BasicCancel(replyConsumerTag);
                }
            }
            finally
            {
                _channelPool.Return(channel);
            }
        } 

        public void AddConsumer<TConsumer, TRequest, TResponse>(TConsumer consumer) 
            where TConsumer : IConsumer<TRequest, TResponse>
            where TRequest : IRequest
            where TResponse : IResponse
        {
            var channel = _channelPool.Get();

            try
            {
                QueueDeclare<TRequest>(channel);

                var eventBasicConsumer = new EventingBasicConsumer(channel);
                eventBasicConsumer.Received += (model, ea) =>
                {
                    var replyChannel = _channelPool.Get();

                    try
                    {
                        var props = ea.BasicProperties;
                        var replyProps = replyChannel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;
                        var requestContext = Deserialize<TRequest>(ea.Body.ToArray());
                        var response = default(TResponse);

                        try
                        {
                            response = consumer.Consume(requestContext);
                        }
                        finally
                        {
                            byte[] responseBytes = null;

                            if (response != null)
                            {
                                responseBytes = Serialize(response);
                            }

                            replyChannel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                basicProperties: replyProps, body: responseBytes);
                            replyChannel.BasicAck(deliveryTag: ea.DeliveryTag,
                                multiple: false);
                        }
                    }
                    finally
                    {
                        _channelPool.Return(replyChannel);
                    }
                };

                channel.BasicConsume(queue: typeof(TRequest).Name,
                    autoAck: false,
                    consumer: eventBasicConsumer);
            }
            finally
            {
                _channelPool.Return(channel);
            }
        }

        private void QueueDeclare<T>(IModel channel)
        {
            var args = new Dictionary<string, object> {{MessageTtlKey, _messageTtl}};
            channel.QueueDeclare(queue: typeof(T).Name,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: args);
        }

        private static T Deserialize<T>(byte[] param)
        {
            using (var ms = new MemoryStream(param))
            {
                var br = new BinaryFormatter();
                return (T)br.Deserialize(ms);
            }
        }

        private static byte[] Serialize<T>(T param)
        {
            using (var ms = new MemoryStream())
            {
                var br = new BinaryFormatter();
                br.Serialize(ms, param);
                return ms.ToArray();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
