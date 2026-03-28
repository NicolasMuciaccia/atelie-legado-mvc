using System.Collections.Generic;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Services.DTOs;

namespace Atelie.Infrastructure.Abstractions.Repositories
{
    public interface IAlunoRepository : IRepository<Aluno>
    {
        List<Aluno> ListarTodosComFiltro(AlunoFiltroDTO filtro);
        int DesativarTurmaAlunos(long id);
        bool NomeDeAlunoExiste(Aluno aluno);
    }
}
