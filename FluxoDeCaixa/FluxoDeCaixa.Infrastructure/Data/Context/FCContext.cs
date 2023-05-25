using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Infrastructure.Extensions;
using FluxoDeCaixa.Shared.AppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace FluxoDeCaixa.Infrastructure.Data.Context
{
    public class FCContext : DbContext
    {
        private readonly string _collation;
        public FCContext(DbContextOptions<FCContext> dbOptions) : base(dbOptions)
        => ChangeTracker.LazyLoadingEnabled = false;

        public FCContext(IOptions<ConnectionStrings> options, DbContextOptions<FCContext> dbOptions) : this(dbOptions)
       => _collation = options.Value.Collation;

        public DbSet<Lancamento> Lancamentos => Set<Lancamento>();
        public DbSet<LivroCaixa> LivrosCaixas => Set<LivroCaixa>();
        public DbSet<Token> Tokens => Set<Token>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (!string.IsNullOrWhiteSpace(_collation))
                modelBuilder.UseCollation(_collation);

            modelBuilder
                .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
                .RemoveCascadeDeleteConvention();
        }
    }
}
