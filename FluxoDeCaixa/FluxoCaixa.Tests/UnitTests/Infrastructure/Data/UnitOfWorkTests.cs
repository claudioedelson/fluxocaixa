using FluentAssertions;
using FluxoCaixa.Tests.Fixtures;
using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Infrastructure.Data;
using FluxoDeCaixa.Infrastructure.Data.Context;
using FluxoDeCaixa.Shared.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Categories;

namespace FluxoCaixa.Tests.UnitTests.Infrastructure.Data
{
    [UnitTest]
    public class UnitOfWorkTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public UnitOfWorkTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Should_ReturnRowsAffected_WhenSaveChanges()
        {
            // Arrange
            var unitOfWork = CreateUoW();
            var context = GetContext();
            context.Add(new LivroCaixa(DateTime.Now,10));

            // Act
            var actual = await unitOfWork.CommitAsync();

            // Assert
            actual.Should().BePositive();
        }

        private FCContext GetContext()
        {
            _fixture.Context.ChangeTracker.Clear();
            return _fixture.Context;
        }

        private IUnitOfWork CreateUoW() => new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());
    }
}
