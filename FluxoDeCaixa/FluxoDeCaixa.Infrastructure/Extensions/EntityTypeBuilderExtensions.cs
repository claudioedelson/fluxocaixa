using FluxoDeCaixa.Shared.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Infrastructure.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        /// <summary>
        /// Configuração da entidade base <see cref="BaseEntity" />.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="builder"></param>
        public static void ConfigureBaseEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
            where TEntity : BaseEntity
        {
            builder.HasKey(entity => entity.Id);

            builder.Property(entity => entity.Id)
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}
