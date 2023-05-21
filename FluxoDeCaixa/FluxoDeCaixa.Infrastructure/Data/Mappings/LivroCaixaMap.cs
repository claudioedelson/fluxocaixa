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
    public class LivroCaixaMap : IEntityTypeConfiguration<LivroCaixa>
    {
        public void Configure(EntityTypeBuilder<LivroCaixa> builder)
        {
            builder.ConfigureBaseEntity();

            builder.ToTable("livrocaixa");
            builder.Property(prop => prop.DataMovimento)
                .HasColumnType("Date")
                .IsRequired();

            builder.Property(prop => prop.SaldoAnterior)
                .HasPrecision(14, 2)
                .IsRequired();

            builder.HasMany(h => h.Lancamentos)
                .WithOne(t => t.LivroCaixa);
           
        }
    }
}
