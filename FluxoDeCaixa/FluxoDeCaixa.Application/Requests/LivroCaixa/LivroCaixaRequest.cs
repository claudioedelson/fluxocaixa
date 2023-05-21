using FluxoDeCaixa.Application.Requests.AuthenticationRequests;
using FluxoDeCaixa.Shared;
using FluxoDeCaixa.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Application.Requests.LivroCaixa
{
    public class LivroCaixaRequest : BaseRequestWithValidation
    {
        public override async Task ValidateAsync()
            => ValidationResult = await LazyValidator.ValidateAsync<LivroCaixaRequestValidator>(this);
    }
}
