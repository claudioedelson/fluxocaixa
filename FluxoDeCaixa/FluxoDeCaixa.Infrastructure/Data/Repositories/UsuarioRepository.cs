using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Domain.Repositories;
using FluxoDeCaixa.Domain.ValuesObjects;
using FluxoDeCaixa.Infrastructure.Data.Context;
using FluxoDeCaixa.Infrastructure.Data.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : EfRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(FCContext context) : base(context)
        {
        }

        public override async Task<Usuario> GetByIdAsync(Guid id, bool readOnly = false)
        {
            var query = readOnly ? DbSet.AsNoTracking() : DbSet.AsQueryable();
            return await query
                .Include(u => u.Tokens.OrderByDescending(t => t.ExpiraEm))
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Usuario> ObterPorEmailAsync(string email)
            => await DbSet
                .Include(u => u.Tokens.OrderByDescending(t => t.ExpiraEm))
                .FirstOrDefaultAsync(u => u.Email == email);

        public async Task<Usuario> ObterPorTokenAtualizacaoAsync(string tokenAtualizacao)
            => await DbSet
                .Include(u => u.Tokens.Where(t => t.Atualizacao == tokenAtualizacao))
                .FirstOrDefaultAsync(u => u.Tokens.Any(t => t.Atualizacao == tokenAtualizacao));

        public async Task<bool> VerificarSeEmailExisteAsync(string email)
            => await DbSet.AsNoTracking().AnyAsync(u => u.Email == email);
    }
}
