using FluxoDeCaixa.Shared.Messages;

namespace FluxoDeCaixa.Application.Responses
{
    public class LivroCaixaResponse : IResponse
    {
        public decimal SaldoAnterior { get; set; }
        public decimal ValorTotal { get; set; }
        public DateTime DataMovimento { get; set; }

        public IList<LancamentoResponse> Lancamentos { get; set; }
            
    }
}
