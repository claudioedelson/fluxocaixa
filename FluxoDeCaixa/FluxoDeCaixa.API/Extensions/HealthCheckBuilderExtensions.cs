using FluxoDeCaixa.API.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.API.Extensions
{
    public static class HealthCheckBuilderExtensions
    {
        public static IHealthChecksBuilder AddGCInfoCheck(this IHealthChecksBuilder builder)
            => builder.AddCheck<GCInfoHealthCheck>("GCInfo", HealthStatus.Degraded, tags: new[] { "memory" });
    }
}
