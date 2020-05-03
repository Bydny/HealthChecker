using HealthChecker.Api.Infrastructure;
using HealthChecker.Api.Models;
using HealthChecker.Api.Services.Interfaces;
using HealthChecker.Contracts.Interfaces.Responses;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace HealthChecker.Api.Services.SignalR
{
    public class HealthCheckService : IHealthCheckService
    {
        private readonly IMemoryCache _memoryCache;

        public HealthCheckService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public HealthCheckVIewModel GetWestLatestInfo()
        {
            if (_memoryCache.TryGetValue(CacheKeys.WestPing,
                out HealthCheckVIewModel pingResult))
            {
                return pingResult;
            }

            return null;
        }

        public void SetWestLatestInfo(IWestResponse model)
        {
            _memoryCache.Set(
                CacheKeys.WestPing,
                new HealthCheckVIewModel(DateTime.Now, model != null));
        }

        public HealthCheckVIewModel GetEastLatestInfo()
        {
            if (_memoryCache.TryGetValue(CacheKeys.EastPing,
                out HealthCheckVIewModel pingResult))
            {
                return pingResult;
            }

            return null;
        }

        public void SetEastLatestInfo(IEastResponse model)
        {
            _memoryCache.Set(
                CacheKeys.EastPing,
                new HealthCheckVIewModel(DateTime.Now, model != null));
        }

        public HealthCheckVIewModel GetSouthLatestInfo()
        {
            if (_memoryCache.TryGetValue(CacheKeys.SouthPing,
                out HealthCheckVIewModel pingResult))
            {
                return pingResult;
            }

            return null;
        }

        public void SetSouthLatestInfo(ISouthResponse model)
        {
            _memoryCache.Set(
                CacheKeys.SouthPing,
                new HealthCheckVIewModel(DateTime.Now, model != null));
        }
    }
}
