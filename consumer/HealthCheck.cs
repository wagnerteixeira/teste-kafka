using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace consumer;
public class HealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy));
    }
}
