using System.Collections.Generic;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Services.DTOs;

namespace Atelie.Infrastructure.Abstractions.Services
{
    public interface IPagamentoService : IService<Pagamento>
    {
        List<Pagamento> ListarTodosComFiltro(PagamentoFiltroDTO filtro);
    }
}