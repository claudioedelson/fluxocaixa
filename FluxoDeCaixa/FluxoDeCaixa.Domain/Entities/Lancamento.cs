using FluxoDeCaixa.Domain.Enums;
using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Domain.Entities
{
    public class Lancamento : BaseEntity,IAggregateRoot
    {
        public Lancamento(string descricao, DateTime dataLancamento, decimal valorLancamento, Guid livroCaixaId, TipoLancamento tipoLancamento)
        {
            Descricao = descricao;
            DataLancamento = dataLancamento;
            ValorLancamento = valorLancamento;
            LivroCaixaId = livroCaixaId;
            TipoLancamento = tipoLancamento;
        }

        public string Descricao { get; private init; }
        public DateTime DataLancamento { get; private init; }
        public decimal ValorLancamento { get; private init; }
        public TipoLancamento TipoLancamento { get; private init; }
        public Guid LivroCaixaId {get;private init; }
        public LivroCaixa LivroCaixa { get; private init; }
    }
}
