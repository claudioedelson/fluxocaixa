using FluxoDeCaixa.Domain.Enums;
using FluxoDeCaixa.Shared.Messages;

namespace FluxoDeCaixa.Application.Responses
{
    public class LancamentoResponse : IResponse
    {
        public DateTime DataLancamento { set;  get; }
        public decimal ValorLancamento { set;  get; }
        public TipoLancamento TipoLancamento { set; get; }
        public string Descricao { get; set; }
    }
}
