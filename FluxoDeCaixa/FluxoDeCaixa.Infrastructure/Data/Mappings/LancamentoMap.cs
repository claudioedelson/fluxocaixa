using FluxoDeCaixa.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluxoDeCaixa.Infrastructure.Extensions;

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
