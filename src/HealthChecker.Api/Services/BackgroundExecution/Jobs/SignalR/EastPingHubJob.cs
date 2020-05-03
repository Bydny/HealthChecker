using HealthChecker.Api.Infrastructure;
using HealthChecker.Api.Services.Interfaces;
using HealthChecker.Api.Services.Interfaces.BackgroundExecution.Jobs.SignalR;
using HealthChecker.Api.Services.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace HealthChecker.Api.Services.BackgroundExecution.Jobs.SignalR
{
    public class EastPingHubJob : PerSixSecondJobMinutely, IEastPingHubJob
    {
        private readonly IHubContext<EastHealthCheckHub> _healthCheckHub;
        private readonly IHealthCheckService _healthCheckService;

        public EastPingHubJob(
            IHubContext<EastHealthCheckHub> healthCheckHub,
            IHealthCheckService healthCheckService)
        {
            _healthCheckHub = healthCheckHub;
            _healthCheckService = healthCheckService;
        }

        public void Execute() => base.Execute(() =>
        {
            _healthCheckHub.Clients.All.SendAsync(
                    Constants.EastHealthCheckEvent,
                    _healthCheckService.GetEastLatestInfo())
                .GetAwaiter()
                .GetResult();
        });
    }
}
