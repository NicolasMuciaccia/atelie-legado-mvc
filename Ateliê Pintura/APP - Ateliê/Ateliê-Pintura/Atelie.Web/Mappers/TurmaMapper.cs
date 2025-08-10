using Atelie.Core.Utils;
using Atelie.Domain.Entities;
using Atelie.Web.ViewModels;
using Atelie.Web.ViewModels.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Atelie.Web.Mappers
{
    public static class TurmaMapper
    {
        public static TurmaViewModel ToViewModel(Turma entidade)
        {
            if (entidade == null) return null;

            return new TurmaViewModel
            {
                Id = entidade.Id,
                NomeCompleto = entidade.GetNomeTurma(),
                DiaDaSemana = entidade.DiaDaSemana,
                DiaDaSemanaDisplay = entidade.DiaDaSemana.GetDisplayName(),
                Horario = entidade.Horario,
                Ativo = entidade.Ativo,
                CursoId = entidade.Curso?.Id ?? 0,
                CursoNome = entidade.Curso?.Nome,
                TotalAlunos = entidade.Alunos.Count(),
                Alunos = entidade.Alunos?.Select(aluno => new AlunoSelectListViewModel
                {
                    Id = aluno.Id,
                    Nome = aluno.Nome
                }).ToList() ?? new List<AlunoSelectListViewModel>()
            };
        }

        public static List<TurmaViewModel> ToViewModel(List<Turma> entidades)
        {
            if (entidades == null) return new List<TurmaViewModel>();

            return entidades.Select(ToViewModel).ToList();
        }
    }
}