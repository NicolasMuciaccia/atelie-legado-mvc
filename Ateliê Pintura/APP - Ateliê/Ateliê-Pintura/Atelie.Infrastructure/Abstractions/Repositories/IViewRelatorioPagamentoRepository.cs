using System.Collections.Generic;
using Atelie.Domain.Entities.Views;
using Atelie.Infrastructure.Services.DTOs;

namespace Atelie.Infrastructure.Abstractions.Repositories
{
    public interface IViewRelatorioPagamentoRepository
    {
        List<ViewRelatorioPagamento> ListarTodosComFiltro(ViewRelatorioPagamentoFiltroDTO filtro);
    }
}
