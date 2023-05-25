using FluxoDeCaixa.Infrastructure.Data;
using FluxoDeCaixa.Infrastructure.Services;
using FluxoDeCaixa.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Diagnostics.CodeAnalysis;

namespace FluxoDeCaixa.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class ServicesCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services)
            => services
                .AddScoped<IDateTimeService, DateTimeService>()
                .AddScoped<IHashService, BCryptHashService>()
                .AddScoped<ITokenClaimsService, JwtClaimService>()
                .AddScoped<IUnitOfWork, UnitOfWork>();

        public static void AddMemoryCacheService(this IServiceCollection services)
            => services.AddScoped<ICacheService, MemoryCacheService>();

        public static void AddDistributedCacheService(this IServiceCollection services)
            => services.AddScoped<ICacheService, DistributedCacheService>();

        public static void AddRepositories(this IServiceCollection services)
        {
            // Assembly scanning and decoration extensions for Microsoft.Extensions.DependencyInjection
            // https://github.com/khellang/Scrutor
            services
                .Scan(scan => scan
                    .FromCallingAssembly()
                    .AddClasses(impl => impl.AssignableTo<IRepository>())
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            // The decorator pattern
            // REF: https://andrewlock.net/adding-decorated-classes-to-the-asp.net-core-di-container-using-scrutor/
            //services
            //    .Decorate<IRecebimentoRepository, RecebimentoRepository>()
            //    .Decorate<IPagamentoRepository, PagamentoRepository>();
               
        }
    }
}
