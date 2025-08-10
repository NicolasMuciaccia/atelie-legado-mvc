using System.Collections.Generic;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Services.DTOs;

namespace Atelie.Infrastructure.Abstractions.Services
{
    public interface IAulaService : IService<Aula>
    {
        List<Aula> ListarTodosComFiltro(AulaFiltroDTO filtro);
    }
}