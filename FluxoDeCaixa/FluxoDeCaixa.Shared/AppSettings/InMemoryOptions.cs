using FluxoDeCaixa.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.AppSettings
{
    public sealed class InMemoryOptions : IAppOptions
    {
        public bool Database { get; private init; }
        public bool Cache { get; private init; }
    }
}
