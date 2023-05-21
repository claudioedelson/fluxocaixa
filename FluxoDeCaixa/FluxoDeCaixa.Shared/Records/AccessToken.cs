using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.Records
{
    public sealed record AccessToken(string Token, DateTime CreatedAt, DateTime ExpiresAt);
}
