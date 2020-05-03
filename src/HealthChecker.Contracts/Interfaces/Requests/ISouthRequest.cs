using HealthChecker.ServiceBus.Interfaces;

namespace HealthChecker.Contracts.Interfaces.Requests
{
    public interface ISouthRequest : IRequest
    {
        string Reason { get; set; }
    }
}
