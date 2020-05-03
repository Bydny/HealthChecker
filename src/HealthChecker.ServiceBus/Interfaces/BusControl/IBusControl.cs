using System;

namespace HealthChecker.ServiceBus.Interfaces.BusControl
{
    public interface IBusControl : IDisposable
    {
        TResponse Send<TRequest, TResponse>(TRequest message)
            where TRequest : IRequest
            where TResponse : IResponse;

        void AddConsumer<TConsumer, TRequest, TResponse>(TConsumer consumer)
            where TConsumer : IConsumer<TRequest, TResponse>
            where TRequest : IRequest
            where TResponse : IResponse;
    }
}
