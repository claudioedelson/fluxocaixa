﻿using FluxoDeCaixa.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Infrastructure.Data.Repositories.Common
{
    public abstract class RepositoryBase<TEntity> : IDisposable where TEntity : class
    {
        protected readonly DbSet<TEntity> DbSet;
        private readonly FCContext _context;

        protected RepositoryBase(FCContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }

        #region IDisposable

        // To detect redundant calls.
        private bool _disposed;

        // Public implementation of Dispose pattern callable by consumers.
        ~RepositoryBase()
        {
            Dispose(false);
        }

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            // Dispose managed state (managed objects).
            if (disposing)
                _context.Dispose();

            _disposed = true;
        }

        #endregion
    }
}