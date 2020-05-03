using Hangfire;
using HealthChecker.Api.Services.Interfaces.BackgroundExecution;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HealthChecker.Api.Services.BackgroundExecution
{
    internal class JobScheduler : IJobScheduler
    {
        private readonly IServiceProvider _serviceProvider;

        public JobScheduler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ScheduleRecurringJob<TJob>(string cronExpression) where TJob : IJob
        {
            var job = _serviceProvider.GetService<TJob>();
            RecurringJob.AddOrUpdate(() => job.Execute(), () => cronExpression);
        }
    }
}
