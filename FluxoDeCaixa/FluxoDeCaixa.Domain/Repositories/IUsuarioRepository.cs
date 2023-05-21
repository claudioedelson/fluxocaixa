using FluxoDeCaixa.Domain.Entities;
using FluxoDeCaixa.Domain.ValuesObjects;
using FluxoDeCaixa.Shared.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Domain.Repositories
{
    public interface IUsuarioRepository : IAsyncRepository<Usuario>
    {
        Task<Usuario> ObterPorEmailAsync(string email);
        Task<Usuario> ObterPorTokenAtualizacaoAsync(string tokenAtualizacao);
        Task<bool> VerificarSeEmailExisteAsync(string email);
    }
}
