using FluxoDeCaixa.Shared.Abstractions;
using FluxoDeCaixa.Shared.AppSettings;
using FluxoDeCaixa.Shared.Constants;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace FluxoDeCaixa.Shared
{
    [ExcludeFromCodeCoverage]
    public static class ServicesCollectionExtensions
    {
        public static void ConfigureAppSettings(this IServiceCollection services)
        {
            services.AddOptions<AuthOptions>(AppSettingsKeys.AuthOptions);
            services.AddOptions<CacheOptions>(AppSettingsKeys.CacheOptions);
            services.AddOptions<ConnectionStrings>(AppSettingsKeys.ConnectionStrings);
            services.AddOptions<InMemoryOptions>(AppSettingsKeys.InMemoryOptions);
            services.AddOptions<JwtOptions>(AppSettingsKeys.JwtOptions);
        }

        private static void AddOptions<TOptions>(this IServiceCollection services, string configSectionPath)
            where TOptions : class, IAppOptions
        {
            services
                .AddOptions<TOptions>()
                .BindConfiguration(configSectionPath, options => options.BindNonPublicProperties = true)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
