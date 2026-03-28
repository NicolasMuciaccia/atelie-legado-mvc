using System.Collections.Generic;
using Atelie.Domain.Entities.Views;
using Atelie.Infrastructure.Services.DTOs;

namespace Atelie.Infrastructure.Abstractions.Services
{
    public interface IViewRelatorioPagamentoService
    {
        List<ViewRelatorioPagamento> ListarTodosComFiltro(ViewRelatorioPagamentoFiltroDTO filtro);
        GraficoPagamentosDTO ObterDadosGrafico(ViewRelatorioPagamentoFiltroDTO filtro);
    }
}
