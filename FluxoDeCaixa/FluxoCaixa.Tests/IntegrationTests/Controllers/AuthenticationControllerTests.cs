using FluentAssertions;
using FluxoCaixa.Tests.Extensions;
using FluxoCaixa.Tests.Fixtures;
using FluxoDeCaixa.Application.Requests.AuthenticationRequests;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Domain.ValuesObjects;
using FluxoDeCaixa.Infrastructure.Data.Context;
using FluxoDeCaixa.Shared.Abstractions;
using FluxoDeCaixa.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FluxoCaixa.Tests.IntegrationTests.Controllers
{
    public class AuthenticationControllerTests : IntegrationTestBase, IClassFixture<WebTestApplicationFactory>
    {
        private const string SenhaPadrao = "@JiL8@cUA%pV";

        public AuthenticationControllerTests(WebTestApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Devera_RetornarResultadoSucessoComToken_AoAutenticar()
        {
            // Arrange
            const string endPoint = "/api/auth/authenticate";
            var usuario = await CriarUsuarioAsync();
            var request = new LogInRequest(usuario.Email, SenhaPadrao);
            var httpContent = new StringContent(request.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);

            // Act
            var act = await HttpClient.PostAsync<TokenResponse>(endPoint, httpContent);

            // Assert
            act.Should().NotBeNull();
            act.AccessToken.Should().NotBeNullOrWhiteSpace();
            act.RefreshToken.Should().NotBeNullOrWhiteSpace();
            act.ExpiresIn.Should().BePositive();
            act.Expiration.Should().BeAfter(act.Created);
            act.Created.Should().BeSameDateAs(DateTime.Now);
        }

        private async Task<Usuario> CriarUsuarioAsync()
        {
            await using var sp = ServiceProvider.CreateAsyncScope();
            await using var context = sp.ServiceProvider.GetRequiredService<FCContext>();
            var hashService = sp.ServiceProvider.GetRequiredService<IHashService>();
            var usuario = new Usuario("convidado", "convidado@guest.com", hashService.Hash(SenhaPadrao));
            context.Add(usuario);
            await context.SaveChangesAsync();
            return usuario;
        }
    }
}
