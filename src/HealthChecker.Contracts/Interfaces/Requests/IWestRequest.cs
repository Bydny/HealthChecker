using HealthChecker.ServiceBus.Interfaces;

namespace HealthChecker.Contracts.Interfaces.Requests
{
    public interface IWestRequest : IRequest
    {
        string Reason { get; set; } 
    }
}
