using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Domain.Enums;
using FluxoDeCaixa.Domain.Repositories;
using FluxoDeCaixa.Infrastructure.Data.Context;
using FluxoDeCaixa.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace FluxoDeCaixa.Infrastructure.Data.Repositories
{
    public class LivroCaixaRepository : EfRepository<LivroCaixa>, ILivroCaixaRepository
    {
        private FCContext _context;
        public LivroCaixaRepository(FCContext context) : base(context)
        {
            _context = context;
        }

        public async Task<LivroCaixa> Obter(DateTime data) => await DbSet
                .Include(u => u.Lancamentos)
                .FirstOrDefaultAsync(u => u.DataMovimento == data);

        public async Task<LivroCaixa> ObterCriar(DateTime data)
        {
           var livroAtual = await DbSet
                .Include(u => u.Lancamentos)
                .FirstOrDefaultAsync(u => u.DataMovimento == data);
            if (livroAtual == null) 
            {
                var ultimoLivro = await DbSet
                .Include(u => u.Lancamentos)
                .FirstOrDefaultAsync(u => u.DataMovimento < data);
                if (ultimoLivro == null)
                {
                    livroAtual = new LivroCaixa(data, 0);
                }
                else {
                    var valorEntradas = ultimoLivro.Lancamentos.Where(s=> s.TipoLancamento == TipoLancamento.Entrada).Sum(s => s.ValorLancamento);
                    var valorSaidas = ultimoLivro.Lancamentos.Where(s => s.TipoLancamento == TipoLancamento.Saida).Sum(s => s.ValorLancamento);
                    var valorAbertura = ultimoLivro.SaldoAnterior;

                    var valorAberturaAtual = valorAbertura + valorEntradas - valorSaidas;
                    livroAtual = new LivroCaixa(data, valorAberturaAtual);
                }
                Add(livroAtual);
                await _context.SaveChangesAsync();
            }

            return livroAtual;
        }
    }
}
