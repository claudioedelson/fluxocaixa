using FluxoDeCaixa.Infrastructure.Data.Context;
using FluxoDeCaixa.Shared.AppSettings;
using FluxoDeCaixa.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FluxoDeCaixa.API.Extensions
{
    internal static class DbContextExtensions
    {
        internal static void AddFCContext(this IServiceCollection services, IHealthChecksBuilder healthChecksBuilder)
        {
            services.AddDbContext<FCContext>((serviceProvider, options) =>
            {
            var inMemoryOptions = serviceProvider.GetOptions<InMemoryOptions>();
            if (inMemoryOptions.Database)
            {
                options.UseInMemoryDatabase($"InMemory_{nameof(FCContext)}");
            }
            else
            {
                var connections = serviceProvider.GetOptions<ConnectionStrings>();

                    options.UseSqlServer(connections.Database, sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
                       
                        // Configurando a resiliência da conexão: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                        sqlOptions.EnableRetryOnFailure(3);
                    });
                }

                var logger = serviceProvider.GetRequiredService<ILogger<FCContext>>();

                // Log tentativas de repetição
                options.LogTo(
                    (eventId, _) => eventId.Id == CoreEventId.ExecutionStrategyRetrying,
                    eventData =>
                    {
                        if (eventData is not ExecutionStrategyEventData retryEventData)
                            return;

                        var exceptions = retryEventData.ExceptionsEncountered;
                        var count = exceptions.Count;
                        var delay = retryEventData.Delay;
                        var message = exceptions[^1].Message;
                        logger.LogWarning("----- Retry #{Count} with delay {Delay} due to error: {Message}", count, delay,
                            message);
                    });

                // Quando for ambiente de desenvolvimento será logado informações detalhadas.
                var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
                if (environment.IsDevelopment())
                    options.EnableDetailedErrors().EnableSensitiveDataLogging();
            });

            // Verificador de saúde da base de dados.
            healthChecksBuilder.AddDbContextCheck<FCContext>(
                tags: new[] { "database" },
                customTestQuery: (context, cancellationToken) =>
                    context.LivrosCaixas.AsNoTracking().AnyAsync(cancellationToken));
        }
    }
}
