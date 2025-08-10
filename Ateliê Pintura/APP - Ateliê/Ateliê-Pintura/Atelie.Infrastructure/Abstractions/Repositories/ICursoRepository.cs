using Atelie.Domain.Entities;

namespace Atelie.Infrastructure.Abstractions.Repositories
{
    public interface ICursoRepository : IRepository<Curso>
    {
        int SwitchAtivo(Curso curso);
        bool NomeDeCursoExiste(Curso curso);
    }
}
