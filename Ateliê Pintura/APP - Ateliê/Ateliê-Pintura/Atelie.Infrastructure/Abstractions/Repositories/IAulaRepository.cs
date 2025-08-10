using System.Collections.Generic;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Services.DTOs;

namespace Atelie.Infrastructure.Abstractions.Repositories
{
    public interface IAulaRepository : IRepository<Aula>
    {
        List<Aula> ListarTodosComFiltro(AulaFiltroDTO filtro);
    }
}
