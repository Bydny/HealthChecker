using HealthChecker.Contracts.Interfaces.Requests;
using System;

namespace HealthChecker.Contracts.Models.Requests
{
    [Serializable]
    public class SouthRequest : ISouthRequest
    {
        public string Reason { get; set; }
    }
}
