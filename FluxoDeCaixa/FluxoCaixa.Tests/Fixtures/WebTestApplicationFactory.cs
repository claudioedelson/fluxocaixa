using FluxoDeCaixa.Infrastructure.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Environments = FluxoCaixa.Tests.Constants.Environments;

namespace FluxoCaixa.Tests.Fixtures
{
    public class WebTestApplicationFactory : WebApplicationFactory<FluxoDeCaixa.API.Program>
    {
        private SqliteConnection _connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
            => builder
                .UseEnvironment(Environments.Testing)
                .UseDefaultServiceProvider(options => options.ValidateScopes = true)
                .ConfigureTestServices(services => services.RemoveAll<IHostedService>())
                .ConfigureServices(services =>
                {
                    var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<FCContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    _connection = new SqliteConnection(ConnectionString.Sqlite);
                    _connection.Open();

                    services.AddDbContext<FCContext>(options => options.UseSqlite(_connection));

                    var serviceProvider = services.BuildServiceProvider(true);
                    using var serviceScope = serviceProvider.CreateScope();
                    using var context = serviceScope.ServiceProvider.GetRequiredService<FCContext>();
                    context.Database.EnsureCreated();
                });

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _connection?.Dispose();
                _connection = null;
            }

            base.Dispose(disposing);
        }
    }
}
