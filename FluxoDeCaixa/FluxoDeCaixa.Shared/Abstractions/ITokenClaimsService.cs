using FluxoDeCaixa.Shared.Records;
using System.Security.Claims;

namespace FluxoDeCaixa.Shared.Abstractions
{

    public interface ITokenClaimsService
    {
        AccessToken GenerateAccessToken(Claim[] claims);
        string GenerateRefreshToken();
    }
}
