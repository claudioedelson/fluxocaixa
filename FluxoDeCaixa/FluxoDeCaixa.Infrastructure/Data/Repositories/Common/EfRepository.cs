﻿using FluxoDeCaixa.Infrastructure.Data.Context;
using FluxoDeCaixa.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Infrastructure.Data.Repositories.Common
{
    public abstract class EfRepository<TEntity> : RepositoryBase<TEntity>, IAsyncRepository<TEntity>
    where TEntity : BaseEntity, IAggregateRoot
    {
        protected EfRepository(FCContext context) : base(context)
        {
        }

        public void Add(TEntity entity)
            => DbSet.Add(entity);

        public void AddRange(IEnumerable<TEntity> entities)
            => DbSet.AddRange(entities);

        public void Update(TEntity entity)
            => DbSet.Update(entity);

        public void UpdateRange(IEnumerable<TEntity> entities)
            => DbSet.UpdateRange(entities);

        public void Remove(TEntity entity)
            => DbSet.Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities)
            => DbSet.RemoveRange(entities);

        public virtual async Task<TEntity> GetByIdAsync(Guid id, bool readOnly = false)
            => readOnly ? await DbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id) : await DbSet.FindAsync(id);
    }
}
