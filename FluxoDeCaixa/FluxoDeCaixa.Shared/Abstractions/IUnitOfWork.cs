using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Shared.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Salva todas as alterações feitas no contexto do banco de dados.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>O número de linhas afetadas no banco de dados.</returns>
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
