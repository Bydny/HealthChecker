using HealthChecker.Contracts.Interfaces.Requests;
using HealthChecker.Contracts.Interfaces.Responses;
using HealthChecker.Contracts.Models.Responses;
using HealthChecker.ServiceBus.Interfaces;
using Serilog;

namespace HealthChecker.South
{
    internal class SouthernConsumer : IConsumer<ISouthRequest, ISouthResponse>
    {
        public ISouthResponse Consume(ISouthRequest request)
        {
            Log.Information(request.Reason);
            return new SouthResponse();
        }
    }
}
