using FluxoDeCaixa.API.Extensions;
using FluxoDeCaixa.Shared.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.IO.Compression;
using FluxoDeCaixa.Shared;
using FluxoDeCaixa.Application;
using FluxoDeCaixa.Infrastructure;
using StackExchange.Profiling;
using FluxoDeCaixa.API.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using AutoMapper;
using FluxoDeCaixa.Shared.AppSettings;
using System;
using FluxoDeCaixa.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
builder.Services.Configure<KestrelServerOptions>(options => options.AddServerHeader = false);
builder.Services.Configure<MvcNewtonsoftJsonOptions>(options => options.SerializerSettings.Configure());
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());
builder.Services.AddCache(builder.Configuration);
builder.Services.AddApiVersioningAndApiExplorer();
builder.Services.AddOpenApi();
builder.Services.ConfigureAppSettings();
builder.Services.AddJwtBearer(builder.Configuration, builder.Environment.IsProduction());
builder.Services.AddServices();
builder.Services.AddInfrastructure();
builder.Services.AddRepositories();

var healthChecksBuilder = builder.Services.AddHealthChecks().AddGCInfoCheck();

//builder.Services.AddDbContext<FCContext>(sql=> sql.UseSqlServer("Server=localhost;Integrated Security=true;Initial Catalog=FCContext;User Id=sa;Password=pMA63033n6tF;Trusted_Connection=false;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;", b=> b.MigrationsAssembly("FluxoDeCaixa.API")));

builder.Services.AddFCContext(healthChecksBuilder);

// Add services to the container.

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
        options.SuppressModelStateInvalidFilter = true;
    }).AddNewtonsoftJson();

// MiniProfiler for .NET
// https://miniprofiler.com/dotnet/
builder.Services.AddMiniProfiler(options =>
{
    // Route: /profiler/results-index
    options.RouteBasePath = "/profiler";
    options.ColorScheme = ColorScheme.Dark;
    options.EnableServerTimingHeader = true;
    options.EnableDebugMode = builder.Environment.IsDevelopment();
    options.TrackConnectionOpenClose = true;
}).AddEntityFramework();

builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
    options.ValidateOnBuild = true;
});

builder.WebHost.UseKestrel();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseSwaggerAndUI(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
app.UseHealthChecks();
app.UseHttpsRedirection();
app.UseHsts();
app.UseRouting();
app.UseHttpLogging();
app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiniProfiler();
app.MapControllers();

await using var serviceScope = app.Services.CreateAsyncScope();
await using var context = serviceScope.ServiceProvider.GetRequiredService<FCContext>();
var mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();
var inMemoryOptions = serviceScope.ServiceProvider.GetOptions<InMemoryOptions>();

try
{
    app.Logger.LogInformation("----- AutoMapper: Validando os mapeamentos...");

    mapper.ConfigurationProvider.AssertConfigurationIsValid();
    mapper.ConfigurationProvider.CompileMappings();

    app.Logger.LogInformation("----- AutoMapper: Mapeamentos são válidos!");

    if (inMemoryOptions.Database)
    {
        app.Logger.LogInformation("----- Database InMemory: Criando e migrando a base de dados...");
        await context.Database.EnsureCreatedAsync();
    }
    else
    {
        var connectionString = context.Database.GetConnectionString();
        app.Logger.LogInformation("----- SQL Server: {Connection}", connectionString);
        app.Logger.LogInformation("----- SQL Server: Verificando se existem migrações pendentes...");

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            app.Logger.LogInformation("----- SQL Server: Criando e migrando a base de dados...");

            await context.Database.MigrateAsync();

            app.Logger.LogInformation("----- SQL Server: Base de dados criada e migrada com sucesso!");
        }
        else
        {
            app.Logger.LogInformation("----- SQL Server: Migrações estão em dia");
        }
    }

    app.Logger.LogInformation("----- Populando a base de dados...");

    await context.EnsureSeedDataAsync();

    app.Logger.LogInformation("----- Base de dados populada com sucesso!");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Ocorreu uma exceção ao iniciar a aplicação: {Message}", ex.Message);
    throw;
}

app.Logger.LogInformation("----- Iniciado a aplicação...");


app.Run();

#pragma warning disable CA1050
namespace FluxoDeCaixa.API
{
#pragma warning disable S2094
    public class Program
    {
    }
#pragma warning restore S2094
}
#pragma warning restore CA1050
