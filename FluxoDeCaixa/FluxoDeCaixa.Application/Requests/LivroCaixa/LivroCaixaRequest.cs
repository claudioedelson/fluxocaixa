using FluxoDeCaixa.Shared;
using FluxoDeCaixa.Shared.Messages;

namespace FluxoDeCaixa.Application.Requests.LivroCaixa
{
    public class LivroCaixaRequest : BaseRequestWithValidation
    {
        public override async Task ValidateAsync()
            => ValidationResult = await LazyValidator.ValidateAsync<LivroCaixaRequestValidator>(this);
    }
}
