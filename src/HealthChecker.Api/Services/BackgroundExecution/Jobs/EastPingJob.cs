using HealthChecker.Api.Services.Interfaces;
using HealthChecker.Api.Services.Interfaces.BackgroundExecution.Jobs;
using HealthChecker.Contracts.Interfaces.Requests;
using HealthChecker.Contracts.Interfaces.Responses;
using HealthChecker.Contracts.Models.Requests;
using HealthChecker.ServiceBus.Interfaces.BusControl;

namespace HealthChecker.Api.Services.BackgroundExecution.Jobs
{
    internal class EastPingJob : PerSixSecondJobMinutely, IEastPingJob
    {
        private readonly IBusControl _busControl;
        private readonly IHealthCheckService _healthCheckService;
        

        public EastPingJob(
            IBusControl busControl,
            IHealthCheckService healthCheckService)
        {
            _busControl = busControl;
            _healthCheckService = healthCheckService;
        }

        public void Execute() => base.Execute(() =>
        {
            var response = _busControl.Send<IEastRequest, IEastResponse>(new EastRequest {Reason = "ping"});
            _healthCheckService.SetEastLatestInfo(response);
        });
    }
}
