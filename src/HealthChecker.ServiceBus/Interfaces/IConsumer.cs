namespace HealthChecker.ServiceBus.Interfaces
{
    public interface IConsumer<in TRequest, out TResponse>
    {
        TResponse Consume(TRequest request);
    }
}
