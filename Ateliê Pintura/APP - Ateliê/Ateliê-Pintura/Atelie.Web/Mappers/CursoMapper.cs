using Atelie.Domain.Entities;
using Atelie.Web.ViewModels.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Atelie.Web.Mappers
{
    public static class CursoMapper
    {
        public static CursoViewModel ToViewModel(Curso entidade)
        {
            if (entidade == null) return null;

            return new CursoViewModel
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                ValorMensal = entidade.ValorMensal,
                Ativo = entidade.Ativo
            };
        }

        public static List<CursoViewModel> ToViewModel(List<Curso> entidades)
        {
            if (entidades == null) return new List<CursoViewModel>();

            return entidades.Select(ToViewModel).ToList();
        }
    }
}
