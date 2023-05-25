using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Domain.Entities
{
    public class LivroCaixa : BaseEntity, IAggregateRoot
    {
        private IList<Lancamento> _lancamentos;
       

        public LivroCaixa(DateTime dataMovimento, decimal saldoAnterior)
        {
            DataMovimento = dataMovimento;
            SaldoAnterior = saldoAnterior;
            _lancamentos = new List<Lancamento>();
        }

        public DateTime DataMovimento { get; private init; }
        public IReadOnlyCollection<Lancamento> Lancamentos { get { return _lancamentos.ToArray(); } }
        public decimal SaldoAnterior { get; private init; }

        public void AdicionarLancamento(Lancamento lancamento )
        {
            _lancamentos.Add(lancamento);
        }

    }
}
