using Ardalis.Result;
using FluxoDeCaixa.Application.Requests.Lancamento;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Domain.Enums;
using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Application.Interfaces
{
    public interface ILivroCaixaService : IAppService
    {
        Task<Result<LivroCaixaResponse>> ObterAsync(DateTime data);
    }
}
