using Ardalis.Result;
using FluxoDeCaixa.Application.Requests.Lancamento;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Domain.Enums;
using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Application.Interfaces
{
    public interface ILancamentoService : IAppService
    {
        Task<Result<LancamentoResponse>> LancarAsync(LancamentoRequest request, TipoLancamento tipoLancamento);
    }
}
