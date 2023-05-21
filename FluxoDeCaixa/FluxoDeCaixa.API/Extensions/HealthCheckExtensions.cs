using FluxoDeCaixa.Shared.Extensions;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace FluxoDeCaixa.API.Extensions
{
    internal static class HealthCheckExtensions
    {
        internal static void UseHealthChecks(this IApplicationBuilder app)
            => app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    Predicate = _ => true,
                    AllowCachingResponses = false,
                    ResponseWriter = (context, healthReport) => context.Response.WriteAsync(healthReport.ToJson())
                });
    }
}
