﻿using FluxoDeCaixa.Infrastructure.Data.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FluxoCaixa.Tests.Fixtures
{
    public class EfSqliteFixture : IAsyncLifetime, IDisposable
    {
        #region Constructor

        private readonly SqliteConnection _connection;

        public EfSqliteFixture()
        {
            _connection = new SqliteConnection(ConnectionString.Sqlite);
            _connection.Open();

            var builder = new DbContextOptionsBuilder<FCContext>().UseSqlite(_connection);
            Context = new FCContext(builder.Options);
        }

        #endregion

        public FCContext Context { get; }

        #region IAsyncLifetime

        public async Task InitializeAsync()
        {
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
            await Context.EnsureSeedDataAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        #endregion

        #region IDisposable

        // To detect redundant calls.
        private bool _disposed;

        // Public implementation of Dispose pattern callable by consumers.
        ~EfSqliteFixture()
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
            {
                _connection?.Dispose();
                Context?.Dispose();
            }

            _disposed = true;
        }

        #endregion
    }
}
