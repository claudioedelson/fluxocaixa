using FluentAssertions;
using FluxoCaixa.Tests.Constants;
using FluxoCaixa.Tests.Fixtures;
using FluxoDeCaixa.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Xunit.Categories;

namespace FluxoCaixa.Tests.UnitTests.Infrastructure.Data.Context
{
    [UnitTest]
    public class FCContextSeedTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public FCContextSeedTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_ReturnsRowsAffected_WhenEnsureSeedData()
        {
            // Arrange
            var context = _fixture.Context;

            // Act
            await context.EnsureSeedDataAsync();
            var totalUsuarios = await context.Usuarios.AsNoTracking().CountAsync();
          
            // Assert
            totalUsuarios.Should().Be(Totais.Usuarios);
           
        }
    }
}
