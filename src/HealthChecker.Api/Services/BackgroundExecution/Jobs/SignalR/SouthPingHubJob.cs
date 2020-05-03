using HealthChecker.Api.Infrastructure;
using HealthChecker.Api.Services.Interfaces;
using HealthChecker.Api.Services.Interfaces.BackgroundExecution.Jobs.SignalR;
using HealthChecker.Api.Services.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace HealthChecker.Api.Services.BackgroundExecution.Jobs.SignalR
{
    public class SouthPingHubJob : PerSixSecondJobMinutely, ISouthPingHubJob
    {
        private readonly IHubContext<SouthHealthCheckHub> _healthCheckHub;
        private readonly IHealthCheckService _healthCheckService;


        public SouthPingHubJob(
            IHubContext<SouthHealthCheckHub> healthCheckHub,
            IHealthCheckService healthCheckService)
        {
            _healthCheckHub = healthCheckHub;
            _healthCheckService = healthCheckService;
        }

        public void Execute() => base.Execute(() =>
        {
            _healthCheckHub.Clients.All.SendAsync(
                    Constants.SouthHealthCheckEvent,
                    _healthCheckService.GetSouthLatestInfo())
                .GetAwaiter()
                .GetResult();
        });
    }
}
