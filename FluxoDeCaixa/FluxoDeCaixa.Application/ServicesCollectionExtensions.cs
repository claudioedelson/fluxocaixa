using FluxoDeCaixa.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Application
{

    [ExcludeFromCodeCoverage]
    public static class ServicesCollectionExtensions
    {
        private static readonly Assembly[] AssembliesToScan = { Assembly.GetExecutingAssembly() };

        public static void AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.DisableConstructorMapping(), AssembliesToScan, ServiceLifetime.Singleton);

            // Assembly scanning and decoration extensions for Microsoft.Extensions.DependencyInjection
            // https://github.com/khellang/Scrutor
            services.Scan(scan => scan
                .FromAssemblies(AssembliesToScan)
                .AddClasses(impl => impl.AssignableTo<IAppService>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }
    }
}
