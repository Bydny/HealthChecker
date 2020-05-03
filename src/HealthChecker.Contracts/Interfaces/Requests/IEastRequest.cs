using HealthChecker.ServiceBus.Interfaces;

namespace HealthChecker.Contracts.Interfaces.Requests
{
    public interface IEastRequest : IRequest
    {
        string Reason { get; set; }
    }
}
