using System;
using System.Threading;
using System.Threading.Tasks;
using web_advert_api.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace web_advert_api.HelthChecks
{
    public class StorageHealthCheck : IHealthCheck
    {
        private readonly IAnuncioStorage _service;
        public StorageHealthCheck(IAnuncioStorage service)
        {
            _service = service;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool isStorageOk = await _service.Checkhealth();
            return new HealthCheckResult(isStorageOk ? HealthStatus.Healthy : HealthStatus.Unhealthy);
        }
    }
}
