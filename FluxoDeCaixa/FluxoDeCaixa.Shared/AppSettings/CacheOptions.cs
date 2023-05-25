using FluxoDeCaixa.Shared.Abstractions;
using FluxoDeCaixa.Shared.ValidationAttributes;

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
