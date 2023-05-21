using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.Abstractions
{
    public  interface IEntity
    {
    }
    public interface IEntityKey<out TKey> : IEntity where TKey : notnull
    {
        TKey Id { get; }
    }
}
