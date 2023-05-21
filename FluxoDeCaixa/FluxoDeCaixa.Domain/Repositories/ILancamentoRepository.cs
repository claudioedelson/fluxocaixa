using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Domain.Repositories
{
    public interface ILancamentoRepository : IAsyncRepository<Lancamento>
    {  
    }
}
