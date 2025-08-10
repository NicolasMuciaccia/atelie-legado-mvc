using System.Collections.Generic;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Services.DTOs;

namespace Atelie.Infrastructure.Abstractions.Repositories
{
    public interface ITurmaRepository : IRepository<Turma>
    {
        List<Turma> ListarTodosComFiltro(TurmaFiltroDTO filtro);
        List<long> ObterTurmaIdPorCursoId(long id);
        List<Turma> ObterTurmaPorAlunoId(long id);
        List<string> ObterNomesDeCursosAtivosPorCursoId(long cursoId);
        int DesativarEmLote(List<long> ids);
        int DesativarTurmaAlunosEmLote(List<long> ids);
        bool ExisteTurmasRepetidas(Turma turma);
    }
}
