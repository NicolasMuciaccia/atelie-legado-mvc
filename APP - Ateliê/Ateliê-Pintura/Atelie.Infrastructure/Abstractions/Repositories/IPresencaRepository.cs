using System.Collections.Generic;
using Atelie.Domain.Entities;

namespace Atelie.Infrastructure.Abstractions.Repositories
{
    public interface IPresencaRepository : IRepository<Presenca>
    {
        List<Presenca> ListarPorAulaId(long aulaId);
        bool ExisteOutraPresencaDoAlunoNaAula(Presenca presenca);
    }
}
