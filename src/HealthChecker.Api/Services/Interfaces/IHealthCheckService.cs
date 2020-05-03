using HealthChecker.Api.Models;
using HealthChecker.Contracts.Interfaces.Responses;

namespace HealthChecker.Api.Services.Interfaces
{
    public interface IHealthCheckService
    {
        HealthCheckVIewModel GetWestLatestInfo();

        void SetWestLatestInfo(IWestResponse model);

        HealthCheckVIewModel GetEastLatestInfo();

        void SetEastLatestInfo(IEastResponse model);

        HealthCheckVIewModel GetSouthLatestInfo();

        void SetSouthLatestInfo(ISouthResponse model);
    }
}
