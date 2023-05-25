using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Domain.Repositories;
using FluxoDeCaixa.Infrastructure.Data.Context;
using FluxoDeCaixa.Infrastructure.Data.Repositories.Common;

namespace FluxoDeCaixa.Infrastructure.Data.Repositories
{
    public class LancamentoRepository : EfRepository<Lancamento>, ILancamentoRepository
    {
        public LancamentoRepository(FCContext context) : base(context)
        {
        }
    }
}
