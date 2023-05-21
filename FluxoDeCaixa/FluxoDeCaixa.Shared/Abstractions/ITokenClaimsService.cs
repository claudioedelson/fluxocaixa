using FluxoDeCaixa.Shared.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.Abstractions
{

    public interface ITokenClaimsService
    {
        AccessToken GenerateAccessToken(Claim[] claims);
        string GenerateRefreshToken();
    }
}
