using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Remove a delação em cascata de chaves estrangeiras (FK).
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        public static void RemoveCascadeDeleteConvention(this ModelBuilder modelBuilder)
        {
            Guard.Against.Null(modelBuilder, nameof(modelBuilder));

            var foreignKeys = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(entity => entity.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in foreignKeys)
                fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}
