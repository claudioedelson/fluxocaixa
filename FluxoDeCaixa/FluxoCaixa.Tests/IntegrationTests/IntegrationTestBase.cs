using FluxoCaixa.Tests.Fixtures;
using Xunit.Categories;

namespace FluxoCaixa.Tests.IntegrationTests
{
    [IntegrationTest]
    public abstract class IntegrationTestBase
    {
        protected IntegrationTestBase(WebTestApplicationFactory factory)
        {
            HttpClient = factory.CreateClient();
            ServiceProvider = factory.Services;
        }

        protected HttpClient HttpClient { get; }
        protected IServiceProvider ServiceProvider { get; }
    }
}
