using FluxoDeCaixa.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.AppSettings
{
    public sealed class ConnectionStrings : IAppOptions
    {
        /// <summary>
        /// String de conexão com a base de dados relacional.
        /// </summary>
        public string Database { get; private init; } = string.Empty;

        /// <summary>
        /// (Opcional) Definição do Collation da base de dados relacional.
        /// REF: https://learn.microsoft.com/en-us/ef/core/miscellaneous/collations-and-case-sensitivity
        /// </summary>
        public string Collation { get; private init; } = string.Empty;

        /// <summary>
        /// String de conexão com o servidor de Cache.
        /// </summary>
        public string Cache { get; private init; } = string.Empty;
    }
}
