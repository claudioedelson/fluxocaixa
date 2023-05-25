using AutoMapper;
using FluxoDeCaixa.Application.Mapper;
using Xunit.Categories;

namespace FluxoCaixa.Tests.UnitTests.Application
{
    [UnitTest]
    public class DomainToResponseMapperTests
    {
        [Fact]
        public void Should_Mapper_ConfigurationIsValid()
        {
            // Arrange
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile<DomainToResponseMapper>());

            // Act
            var mapper = new Mapper(configuration);

            // Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
