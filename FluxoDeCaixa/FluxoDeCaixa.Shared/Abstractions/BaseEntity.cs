namespace FluxoDeCaixa.Shared.Abstractions
{
    public abstract class BaseEntity : IEntityKey<Guid>
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
    }
}
