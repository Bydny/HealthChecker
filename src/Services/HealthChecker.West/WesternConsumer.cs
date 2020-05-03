using HealthChecker.Contracts.Interfaces.Requests;
using HealthChecker.Contracts.Interfaces.Responses;
using HealthChecker.Contracts.Models.Responses;
using HealthChecker.ServiceBus.Interfaces;
using Serilog;

namespace HealthChecker.West
{
    internal class WesternConsumer : IConsumer<IWestRequest, IWestResponse>
    {
        public IWestResponse Consume(IWestRequest request)
        {
            Log.Information(request.Reason);
            return new WestResponse();
        }
    }
}
