using Atelie.Domain.Entities;
using Atelie.Web.ViewModels.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Atelie.Web.Mappers
{
    public static class AulaMapper
    {
        public static AulaViewModel ToViewModel(Aula entidade)
        {
            if (entidade == null) 
                return null;

            return new AulaViewModel
            {
                Id = entidade.Id,
                Nome = $"{entidade.Turma?.GetNomeTurma()} - {entidade.DataAula}",
                DataAula = entidade.DataAula,
                TurmaId = entidade.Turma?.Id ?? 0,
                TurmaNome = entidade.Turma?.GetNomeTurma(),
                TurmaAtiva = entidade.Turma?.Ativo ?? false,
                TotalPresencas = entidade.Presencas.Count(p => p.Presente),
                TotalFaltas = entidade.Presencas.Count(p => !p.Presente),
                Presencas = entidade.Presencas?.Select(p => new PresencaViewModel
                {
                    Id = p.Id,
                    AlunoId = p.Aluno.Id,
                    AlunoNome = p.Aluno.Nome,
                    Presente = p.Presente
                }).ToList() ?? new List<PresencaViewModel>()
            };
        }

        public static List<AulaViewModel> ToViewModel(List<Aula> entidades)
        {
            if (entidades == null) return new List<AulaViewModel>();

            return entidades.Select(ToViewModel).ToList();
        }
    }
}