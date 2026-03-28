using System.Collections.Generic;
using Atelie.Domain.Entities;

namespace Atelie.Infrastructure.Abstractions.Services
{
    public interface ICursoService : IService<Curso>
    {
        List<long> ObterTurmaIdPorCursoId(long id);
        void SwitchAtivo(long id);
    }
}