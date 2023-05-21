using FluxoDeCaixa.Domain.Enums;
using FluxoDeCaixa.Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
