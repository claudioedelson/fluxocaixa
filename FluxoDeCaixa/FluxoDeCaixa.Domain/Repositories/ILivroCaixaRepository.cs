using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Domain.ValuesObjects;
using FluxoDeCaixa.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Domain.Repositories
{
    public interface ILivroCaixaRepository : IAsyncRepository<LivroCaixa>
    {
        Task<LivroCaixa> Obter(DateTime data);
        Task<LivroCaixa> ObterCriar(DateTime data);
    }
}
