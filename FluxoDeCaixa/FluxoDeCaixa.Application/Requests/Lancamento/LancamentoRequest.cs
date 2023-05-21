using FluxoDeCaixa.Shared;
using FluxoDeCaixa.Shared.Messages;
using System.ComponentModel.DataAnnotations;

namespace FluxoDeCaixa.Application.Requests.Lancamento
{
    public class LancamentoRequest : BaseRequestWithValidation
    {
        [Required]
        public required string Descricao { get; set; }
        [Required]
        public decimal ValorLancamento { get; set; }
        [Required]
        public DateTime DataLancamento { get; set; }
        public override async Task ValidateAsync()
            => ValidationResult = await LazyValidator.ValidateAsync<LancamentoRequestValidator>(this);

    }
}
