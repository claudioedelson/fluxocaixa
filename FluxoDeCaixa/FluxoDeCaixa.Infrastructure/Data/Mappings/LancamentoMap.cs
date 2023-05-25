using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluxoDeCaixa.Infrastructure.Data.Mappings
{
    public class LancamentoMap : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            builder.ConfigureBaseEntity();

            builder.ToTable("lancamento");

            builder.Property(prop => prop.DataLancamento)
                .IsRequired();

            builder.Property(prop => prop.ValorLancamento)
                .IsRequired()
                .HasPrecision(14, 2);
        }
    }
}
