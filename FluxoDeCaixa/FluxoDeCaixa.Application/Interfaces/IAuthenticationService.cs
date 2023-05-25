using Ardalis.Result;
using FluxoDeCaixa.Application.Requests.AuthenticationRequests;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Application.Interfaces
{
    public interface IAuthenticationService : IAppService
    {
        Task<Result<TokenResponse>> AuthenticateAsync(LogInRequest request);
        Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
