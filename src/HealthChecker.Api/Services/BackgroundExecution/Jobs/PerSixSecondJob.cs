using System;
using System.Linq;
using System.Threading;

namespace HealthChecker.Api.Services.BackgroundExecution.Jobs
{
    public abstract class PerSixSecondJobMinutely
    {
        private readonly int _threadSleepTimeout;
        private const int MinutelyCronDivider = 10;


        protected PerSixSecondJobMinutely()
        {
            _threadSleepTimeout = Convert.ToInt32(TimeSpan.FromMinutes(1).TotalMilliseconds / MinutelyCronDivider);
        }

        public void Execute(Action job)
        {
            foreach (var i in Enumerable.Range(1, MinutelyCronDivider))
            {
                job();
                Thread.Sleep(_threadSleepTimeout);
            }
        }
    }
}
