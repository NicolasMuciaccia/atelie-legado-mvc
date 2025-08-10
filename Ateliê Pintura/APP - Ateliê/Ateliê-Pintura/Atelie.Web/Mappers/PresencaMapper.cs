using Atelie.Domain.Entities;
using Atelie.Web.ViewModels.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Atelie.Web.Mappers
{
    public static class PresencaMapper
    {
        public static PresencaViewModel ToViewModel(Presenca entidade)
        {
            if (entidade == null) return null;

            return new PresencaViewModel
            {
                Presente = entidade.Presente,
                AlunoId = entidade.Aluno?.Id ?? 0,
                AlunoNome = entidade.Aluno?.Nome
            };
        }

        public static List<PresencaViewModel> ToViewModel(List<Presenca> entidades)
        {
            if (entidades == null) return new List<PresencaViewModel>();

            return entidades.Select(ToViewModel).ToList();
        }
    }
}