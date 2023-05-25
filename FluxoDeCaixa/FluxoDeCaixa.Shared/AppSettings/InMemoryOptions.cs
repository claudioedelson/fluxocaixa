using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Shared.AppSettings
{
    public sealed class InMemoryOptions : IAppOptions
    {
        public bool Database { get; private init; }
        public bool Cache { get; private init; }
    }
}
