using FluxoDeCaixa.Shared.Abstractions;
using FluxoDeCaixa.Shared.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.AppSettings
{
    public sealed class CacheOptions : IAppOptions
    {
        [RequiredGreaterThanZero]
        public int AbsoluteExpirationInHours { get; private init; }

        [RequiredGreaterThanZero]
        public int SlidingExpirationInSeconds { get; private init; }
    }
}
