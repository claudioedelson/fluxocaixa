using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Application.Requests.Lancamento
{
    public class LancamentoRequestValidator :AbstractValidator<LancamentoRequest>
    {
        public LancamentoRequestValidator() 
        {
            RuleFor(req => req.Descricao)
                .NotEmpty().WithMessage("Informe a descrição do lançamento.")
                .MaximumLength(200);
            RuleFor(req => req.ValorLancamento)
                .GreaterThan(0).WithMessage("Valor de lançamento precisa ser positivo.");
                

        }
    }
}
