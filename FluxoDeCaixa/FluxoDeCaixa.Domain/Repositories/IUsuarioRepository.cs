using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Shared.Abstractions;

namespace FluxoDeCaixa.Domain.Repositories
{
    public interface IUsuarioRepository : IAsyncRepository<Usuario>
    {
        Task<Usuario> ObterPorEmailAsync(string email);
        Task<Usuario> ObterPorTokenAtualizacaoAsync(string tokenAtualizacao);
        Task<bool> VerificarSeEmailExisteAsync(string email);
    }
}
