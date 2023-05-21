using FluxoDeCaixa.Shared.Abstractions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.Extensions
{
    public static class ConfigurationExtensions
    {
        public static TOptions GetOptions<TOptions>(this IConfiguration configuration, string configSectionPath)
            where TOptions : IAppOptions
        {
            return configuration
                .GetSection(configSectionPath)
                .Get<TOptions>(binderOptions => binderOptions.BindNonPublicProperties = true);
        }
    }
}
