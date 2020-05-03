using System;

namespace HealthChecker.Api.Models
{
    public class HealthCheckVIewModel
    {
        public bool IsHealthy { get; set; }
        public DateTime PingTime { get; set; }


        public HealthCheckVIewModel() { }

        public HealthCheckVIewModel(DateTime time, bool isHealthy)
        {
            PingTime = time;
            IsHealthy = isHealthy;
        }
    }
}
