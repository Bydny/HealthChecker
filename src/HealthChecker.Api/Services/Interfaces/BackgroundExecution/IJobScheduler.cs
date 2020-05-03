namespace HealthChecker.Api.Services.Interfaces.BackgroundExecution
{
    public interface IJobScheduler
    {
        void ScheduleRecurringJob<TJob>(string cronExpression) where TJob : IJob;
    }
}
