using HealthChecker.Api.Services.Interfaces;
using HealthChecker.Api.Services.Interfaces.BackgroundExecution.Jobs;
using HealthChecker.Contracts.Interfaces.Requests;
using HealthChecker.Contracts.Interfaces.Responses;
using HealthChecker.Contracts.Models.Requests;
using HealthChecker.ServiceBus.Interfaces.BusControl;

namespace HealthChecker.Api.Services.BackgroundExecution.Jobs
{
    internal class WestPingJob : PerSixSecondJobMinutely, IWestPingJob
    {
        private readonly IBusControl _busControl;
        private readonly IHealthCheckService _healthCheckService;


        public WestPingJob(
            IBusControl busControl,
            IHealthCheckService healthCheckService)
        {
            _busControl = busControl;
            _healthCheckService = healthCheckService;
        }

        public void Execute() => base.Execute(() =>
        {
            var response = _busControl.Send<IWestRequest, IWestResponse>(new WestRequest {Reason = "ping"});
            _healthCheckService.SetWestLatestInfo(response);
        });
    }
}
