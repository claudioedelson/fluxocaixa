using FluxoDeCaixa.Shared.AppSettings;
using FluxoDeCaixa.Shared.Constants;
using FluxoDeCaixa.Infrastructure;
using FluxoDeCaixa.Shared.Extensions;


namespace FluxoDeCaixa.API.Extensions
{
    internal static class CacheExtensions
    {
        internal static void AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            var inMemoryOptions = configuration.GetOptions<InMemoryOptions>(AppSettingsKeys.InMemoryOptions);
            if (inMemoryOptions.Cache)
            {
                services.AddMemoryCache().AddMemoryCacheService();
            }
            else
            {
                var connections = configuration.GetOptions<ConnectionStrings>(AppSettingsKeys.ConnectionStrings);

                services.AddDistributedRedisCache(options =>
                {
                    options.InstanceName = "master";
                    options.Configuration = connections.Cache;
                }).AddDistributedCacheService();
            }
        }
    }
}
