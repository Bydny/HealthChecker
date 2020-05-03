using HealthChecker.Contracts.Interfaces.Requests;
using System;

namespace HealthChecker.Contracts.Models.Requests
{
    [Serializable]
    public class WestRequest : IWestRequest
    {
        public string Reason { get; set; }
    }
}
