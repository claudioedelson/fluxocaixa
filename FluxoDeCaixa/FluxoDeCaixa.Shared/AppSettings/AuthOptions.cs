using FluxoDeCaixa.Shared.Abstractions;
using FluxoDeCaixa.Shared.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.AppSettings
{
    public sealed class AuthOptions : IAppOptions
    {
        [RequiredGreaterThanZero]
        public int MaximumAttempts { get; private init; }

        [RequiredGreaterThanZero]
        public int SecondsBlocked { get; private init; }

        public static AuthOptions Create(int maximumAttempts, int secondsBlocked)
            => new() { MaximumAttempts = maximumAttempts, SecondsBlocked = secondsBlocked };
    }
}
