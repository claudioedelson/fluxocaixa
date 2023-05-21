using Bogus;
using FluentAssertions;
using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Domain.ValuesObjects;
using FluxoDeCaixa.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Categories;

namespace FluxoCaixa.Tests.UnitTests.Domain.Entities
{
    [UnitTest]
    public class UsuarioTests
    {
        [Fact]
        public void Devera_RetornarBloqueioVerdadeiro_QuandoIncrementarFalhasAoLimite()
        {
            // Arrange
            var faker = new Faker();
            var usuario = new Usuario(faker.Person.FullName, faker.Person.Email, faker.Internet.Password());
            var dateTimeService = new DateTimeService();
            const int numeroTentativas = 1;
            usuario.IncrementarFalhas(dateTimeService, numeroTentativas, TimeSpan.FromMinutes(30));

            // Act
            var act = usuario.EstaBloqueado(dateTimeService);

            // Assert
            act.Should().BeTrue();
            usuario.BloqueioExpiraEm.Should().NotBeNull();
            usuario.NumeroFalhasAoAcessar.Should().Be(0);
        }

        [Fact]
        public void Devera_RetornarBloqueioFalso_AoIncrementarFalhas()
        {
            // Arrange
            var faker = new Faker();
            var usuario = new Usuario(faker.Person.FullName, faker.Person.Email, faker.Internet.Password());
            var dateTimeService = new DateTimeService();
            const int numeroTentativas = 3;
            usuario.IncrementarFalhas(dateTimeService, numeroTentativas, TimeSpan.FromMinutes(30));

            // Act
            var act = usuario.EstaBloqueado(dateTimeService);

            // Assert
            act.Should().BeFalse();
            usuario.BloqueioExpiraEm.Should().BeNull();
            usuario.NumeroFalhasAoAcessar.Should().Be(1);
        }
    }
}
