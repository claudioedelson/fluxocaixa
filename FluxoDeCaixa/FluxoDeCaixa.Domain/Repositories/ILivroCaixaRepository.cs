using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Domain.Repositories
{
    public interface ILivroCaixaRepository : IAsyncRepository<LivroCaixa>
    {
        Task<LivroCaixa> Obter(DateTime data);
        Task<LivroCaixa> ObterCriar(DateTime data);
    }
}
