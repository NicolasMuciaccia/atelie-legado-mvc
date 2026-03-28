using System.Collections.Generic;
using Atelie.Domain.Entities;

namespace Atelie.Infrastructure.Abstractions.Services
{
    public interface IPresencaService : IService<Presenca>
    {
        List<Presenca> SincronizarPresencas(List<Presenca> presencas);
        List<Presenca> ListarTodosPorAulaId(long id);
    }
}