using HealthChecker.Contracts.Interfaces.Requests;
using HealthChecker.Contracts.Interfaces.Responses;
using HealthChecker.Contracts.Models.Responses;
using HealthChecker.ServiceBus.Interfaces;
using Serilog;

namespace HealthChecker.East
{
    internal class EasternConsumer : IConsumer<IEastRequest, IEastResponse>
    {
        public IEastResponse Consume(IEastRequest request)
        {
            Log.Information(request.Reason);
            return new EastResponse();
        }
    }
}
