using Ardalis.Result;
using Bogus;
using FluentAssertions;
using FluxoCaixa.Tests.Extensions;
using FluxoCaixa.Tests.Fixtures;
using FluxoDeCaixa.Application.Interfaces;
using FluxoDeCaixa.Application.Requests.AuthenticationRequests;
using FluxoDeCaixa.Application.Services;
using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Domain.Repositories;
using FluxoDeCaixa.Infrastructure.Data;
using FluxoDeCaixa.Infrastructure.Data.Repositories;
using FluxoDeCaixa.Infrastructure.Services;
using FluxoDeCaixa.Shared.Abstractions;
using FluxoDeCaixa.Shared.AppSettings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit.Categories;

namespace FluxoCaixa.Tests.UnitTests.Application.Services
{
    [UnitTest]
    public class AuthenticationServiceTests : IClassFixture<EfSqliteFixture>
    {
        private readonly EfSqliteFixture _fixture;

        public AuthenticationServiceTests(EfSqliteFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Devera_RetornarSucessoComToken_AoAutenticar()
        {
            // Arrange
            var jwtOptions = CreateJwtOptions();
            var dateTime = new DateTimeService();
            var tokenClaimsService = new JwtClaimService(jwtOptions, dateTime);
            var hashService = new BCryptHashService(Mock.Of<ILogger<BCryptHashService>>());
            var usuarioRepository = new UsuarioRepository(_fixture.Context);
            var unitOfWork = new UnitOfWork(_fixture.Context, Mock.Of<ILogger<UnitOfWork>>());
            var service = CreateAuthenticationService(
                CreateAuthOptions(),
                dateTime,
                hashService,
                tokenClaimsService,
                usuarioRepository,
                unitOfWork);

            const string nome = "User Test";
            const string email = "user_teste@hotmail.com";
            const string senha = "VWBMx1bVqP01";
            usuarioRepository.Add(new Usuario(nome, email, hashService.Hash(senha)));
            await unitOfWork.CommitAsync();
            var request = new LogInRequest(email, senha);

            // Act
            var actual = await service.AuthenticateAsync(request);

            // Assert
            actual.Should().NotBeNull();
            actual.IsSuccess.Should().BeTrue();
            actual.Value.Should().NotBeNull();
            var tokenResponse = actual.Value;
            tokenResponse.AccessToken.Should().NotBeNullOrWhiteSpace();
            tokenResponse.Expiration.Should().BeAfter(tokenResponse.Created);
            tokenResponse.RefreshToken.Should().NotBeNullOrWhiteSpace();
            tokenResponse.ExpiresIn.Should().BePositive().And.Be(jwtOptions.Value.Seconds);
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_AoAutenticarContaBloqueada()
        {
            // Arrange
            const string expectedError = "A sua conta está bloqueada, entre em contato com o nosso suporte.";

            var usuario = new Faker<Usuario>()
                .UsePrivateConstructor()
                .RuleFor(usuario => usuario.Id, faker => faker.Random.Guid())
                .RuleFor(usuario => usuario.Nome, faker => faker.Person.FullName)
                .RuleFor(usuario => usuario.Email, faker => faker.Person.Email)
                .RuleFor(usuario => usuario.HashSenha, faker => faker.Internet.Password())
                .RuleFor(usuario => usuario.UltimoAcessoEm, faker => faker.Date.Past())
                .RuleFor(usuario => usuario.BloqueioExpiraEm, faker => faker.Date.Future())
                .RuleFor(usuario => usuario.NumeroFalhasAoAcessar, faker => faker.Random.Int(1, 10))
                .Generate();

            var logInRequest = new LogInRequest(usuario.Email, usuario.HashSenha);

            var usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            usuarioRepositoryMock
                .Setup(s => s.ObterPorEmailAsync(It.IsNotNull<string>()))
                .ReturnsAsync(usuario)
                .Verifiable();

            var dateTimeMock = new Mock<IDateTimeService>();
            dateTimeMock.SetupGet(s => s.Now).Returns(DateTime.Now).Verifiable();

            var service = CreateAuthenticationService(
                dateTimeService: dateTimeMock.Object,
                usuarioRepository: usuarioRepositoryMock.Object);

            // Act
            var actual = await service.AuthenticateAsync(logInRequest);

            // Assert
            actual.Should().NotBeNull();
            actual.IsSuccess.Should().BeFalse();
            actual.Status.Should().Be(ResultStatus.Error);
            actual.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.SatisfyRespectively(error => error.Should().NotBeNullOrWhiteSpace().And.Be(expectedError));

            usuarioRepositoryMock.Verify(s => s.ObterPorEmailAsync(It.IsNotNull<string>()), Times.Once);
            dateTimeMock.Verify(s => s.Now, Times.Once);
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_AoAutenticarLogInInvalido()
        {
            // Arrange
            var logInRequest = new LogInRequest(string.Empty, string.Empty);
            var service = CreateAuthenticationService();

            // Act
            var actual = await service.AuthenticateAsync(logInRequest);

            // Assert
            actual.Should().NotBeNull();
            actual.IsSuccess.Should().BeFalse();
            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.ValidationErrors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.Subject.ForEach(error => error.ErrorMessage.Should().NotBeNullOrWhiteSpace());
        }

        [Fact]
        public async Task Devera_RetornarErroValidacao_AoAutenticarLogInInexistente()
        {
            // Arrange
            const string expectedError = "A conta informada não existe.";
            var logInRequest = new LogInRequest("joao.ninguem@gmail.com", "gUoCA3#d1oKB");
            var service = CreateAuthenticationService();

            // Act
            var actual = await service.AuthenticateAsync(logInRequest);

            // Assert
            actual.Should().NotBeNull();
            actual.IsSuccess.Should().BeFalse();
            actual.Status.Should().Be(ResultStatus.NotFound);
            actual.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.SatisfyRespectively(error => error.Should().NotBeNullOrWhiteSpace().And.Be(expectedError));
        }

        private static IAuthenticationService CreateAuthenticationService(
            IOptions<AuthOptions> authOptions = null,
            IDateTimeService dateTimeService = null,
            IHashService hashService = null,
            ITokenClaimsService tokenClaimsService = null,
            IUsuarioRepository usuarioRepository = null,
            IUnitOfWork unitOfWork = null)
        {
            return new AuthenticationService(
                authOptions ?? Mock.Of<IOptions<AuthOptions>>(),
                dateTimeService ?? Mock.Of<IDateTimeService>(),
                hashService ?? Mock.Of<IHashService>(),
                tokenClaimsService ?? Mock.Of<ITokenClaimsService>(),
                usuarioRepository ?? Mock.Of<IUsuarioRepository>(),
                unitOfWork ?? Mock.Of<IUnitOfWork>());
        }

        private static IOptions<AuthOptions> CreateAuthOptions()
        {
            const short maximumAttempts = 3;
            const short secondsBlocked = 1000;
            return Options.Create(AuthOptions.Create(maximumAttempts, secondsBlocked));
        }

        private static IOptions<JwtOptions> CreateJwtOptions()
        {
            const string audience = "Clients-API-FC";
            const string issuer = "API-FC";
            const string secretKey = "p8SXNddEAEn1cCuyfVJKYA7e6hlagbLd";
            const short seconds = 21600;
            var jwtConfig = JwtOptions.Create(audience, issuer, seconds, secretKey);
            return Options.Create(jwtConfig);
        }
    }
}
