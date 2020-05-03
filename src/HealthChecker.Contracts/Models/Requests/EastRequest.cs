using HealthChecker.Contracts.Interfaces.Requests;
using System;

namespace HealthChecker.Contracts.Models.Requests
{
    [Serializable]
    public class EastRequest : IEastRequest
    {
        public string Reason { get; set; }
    }
}
