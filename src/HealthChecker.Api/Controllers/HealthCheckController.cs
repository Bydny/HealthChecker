using Hangfire;
using HealthChecker.Api.Services.Interfaces.BackgroundExecution;
using HealthChecker.Api.Services.Interfaces.BackgroundExecution.Jobs.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace HealthChecker.Api.Controllers
{
    [Route("api/health-check")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly IJobScheduler _jobScheduler;


        public HealthCheckController(IJobScheduler jobScheduler)
        {
            _jobScheduler = jobScheduler;
        }

        [HttpGet("west")]
        public IActionResult CheckWest()
        {
            _jobScheduler.ScheduleRecurringJob<IWestPingHubJob>(Cron.Minutely());
            return Ok();
        }

        [HttpGet("east")]
        public IActionResult CheckEast()
        {
            _jobScheduler.ScheduleRecurringJob<IEastPingHubJob>(Cron.Minutely());
            return Ok();
        }

        [HttpGet("south")]
        public IActionResult CheckSouth()
        {
            _jobScheduler.ScheduleRecurringJob<ISouthPingHubJob>(Cron.Minutely());
            return Ok();
        }
    }
}