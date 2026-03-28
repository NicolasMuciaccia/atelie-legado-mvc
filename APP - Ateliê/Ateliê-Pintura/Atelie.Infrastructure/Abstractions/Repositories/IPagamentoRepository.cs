using System.Collections.Generic;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Services.DTOs;

namespace Atelie.Infrastructure.Abstractions.Repositories
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        List<Pagamento> ListarTodosComFiltro(PagamentoFiltroDTO filtro);
        bool ExisteOutroPagamentoNoMesmoMes(Pagamento pagamento);
    }
}
