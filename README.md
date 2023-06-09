# Sistema Fluxo de Caixa (SFC)

[.NET 7](https://docs.microsoft.com/pt-br/dotnet/core/whats-new/dotnet-7) + [EF Core 7.0](https://docs.microsoft.com/pt-br/ef/core/what-is-new/ef-core-7.0/whatsnew) + JWT Bearer + OpenAPI (Swagger)

> Nota: projeto focado em **Back-End**

- RESTful API
- Banco de dados relacional: **SQL Server**
- Cache Distribuído: **Redis**
- Clean Architecture
- Princípios **S.O.L.I.D.**
- Conceitos de modelagem de software **DDD (Domain Driven Design)**
- Padrão Repository-Service (Repository-Service Pattern)
- Padrão decorador (decorator pattern) [The decorator pattern](https://andrewlock.net/adding-decorated-classes-to-the-asp.net-core-di-container-using-scrutor/)
- Padrão de Camada-Anticorrupção (Anti-Corruption Layer) **(FluentValidation)**
- Padrão Resultado **(FluentResults)** [Functional C#: Handling failures](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/)
- [Scrutor](https://github.com/khellang/Scrutor) automaticamente registrando os serviços no ASP.NET Core DI
- Testes Unitários, Integrações com **xUnit**, **FluentAssertions**, **Moq**\
    => [Melhores práticas de teste de unidade com .NET Core](https://docs.microsoft.com/pt-br/dotnet/core/testing/unit-testing-best-practices)
- Monitoramento de performance da aplicação: [MiniProfiler for .NET](https://miniprofiler.com/dotnet/)
- Verificações de integridade da aplicação com [HealthChecks](https://docs.microsoft.com/pt-br/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-6.0)

## Desenho da Arquitetura baseada em Clean Code
[![Arquitetura de Software](https://github.com/claudioedelson/fluxocaixa/blob/main/Imagens/Arquitetura.png)]


## Executando a aplicação usando o Docker
-  Pré requisito Docker instalado - [![Docker] <img src="https://github.com/claudioedelson/fluxocaixa/blob/main/Imagens/docker.svg" width="32">](https://www.docker.com/)
- Usuário de autenticação 
    - usuario: guest@guest.com
    - senha  : "123456"


Após executar o comando no terminal `docker-compose up --build` na raiz do repositório, basta abrir a url no navegador: `http://localhost:8000/swagger/`

## MiniProfiler for .NET

Para acessar a página de monitoramento de requisições e performance:
`http://localhost:8000/profiler/results-index`

## HealthCheck for .NET

Para acessar a página de verificação da saúde da aplicação com os seguintes indicadores; GC,Base de Dados  :
`http://localhost:8000/health`

## Configurando Banco de dados

Por padrão é utilizado o SQL Server LocalDB, para alterar a conexão, modifique o valor da chave `Database` no arquivo `appsettings.json`

```json
{
  "ConnectionStrings": {
    "Database": "Server=(localdb)\\mssqllocaldb;Database=FCContext;Trusted_Connection=True;MultipleActiveResultSets=true;",
    "Collation": "Latin1_General_CI_AI"
  }
}
```

Ao iniciar a aplicação o banco de dados será criado automaticamente e efetuado as migrações pendentes,
também será populado o arquivo de seed.

```c#
await using var serviceScope = app.Services.CreateAsyncScope();
await using var context = serviceScope.ServiceProvider.GetRequiredService<SgpContext>();
var mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();
var inMemoryOptions = serviceScope.ServiceProvider.GetOptions<InMemoryOptions>();

try
{
    app.Logger.LogInformation("----- AutoMapper: Validando os mapeamentos...");

    mapper.ConfigurationProvider.AssertConfigurationIsValid();
    mapper.ConfigurationProvider.CompileMappings();

    app.Logger.LogInformation("----- AutoMapper: Mapeamentos são válidos!");

    if (inMemoryOptions.Cache)
    {
        app.Logger.LogInformation("----- Cache: InMemory");
    }
    else
    {
        app.Logger.LogInformation("----- Cache: Distributed");
    }

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
            app.Logger.LogInformation("----- SQL Server: Migrações estão em dia.");
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
```

## License

- [MIT License](https://github.com/claudioedelson/fluxocaixa/blob/main/LICENSE)
