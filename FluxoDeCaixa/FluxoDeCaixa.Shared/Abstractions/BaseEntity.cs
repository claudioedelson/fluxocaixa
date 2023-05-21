using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.Abstractions
{
    public abstract class BaseEntity : IEntityKey<Guid>
    {
        public Guid Id { get; private init; } = Guid.NewGuid();
    }
}
