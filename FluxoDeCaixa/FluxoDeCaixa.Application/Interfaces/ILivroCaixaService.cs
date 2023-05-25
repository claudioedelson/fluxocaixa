using Ardalis.Result;
using FluxoDeCaixa.Application.Responses;
using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Application.Interfaces
{
    public interface ILivroCaixaService : IAppService
    {
        Task<Result<LivroCaixaResponse>> ObterAsync(DateTime data);
    }
}
