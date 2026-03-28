using Atelie.Core.Utils;
using Atelie.Domain.Entities;
using Atelie.Web.ViewModels.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Atelie.Web.Mappers
{
    public static class AlunoMapper
    {
        public static AlunoViewModel ToViewModel(Aluno entidade, int mesFiltro)
        {
            if (entidade == null) 
                return null;

            return new AlunoViewModel
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                TipoContato = entidade.TipoContato,
                DescricaoContato = entidade.DescricaoContato,
                DiaPagamentoPreferencial = entidade.DiaPagamentoPreferencial,
                Ativo = entidade.Ativo,
                PresencaMensal = entidade.Presencas.Count(p => p.Aula.DataAula.Month == mesFiltro && p.Aula.DataAula.Year == System.DateTime.Now.Year && p.Presente),
                TipoContatoDisplay = entidade.TipoContato.GetDisplayName()
            };
        }

        public static AlunoViewModel ToViewModel(Aluno entidade)
        {
            if (entidade == null) return null;

            return new AlunoViewModel
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                TipoContato = entidade.TipoContato,
                DescricaoContato = entidade.DescricaoContato,
                DiaPagamentoPreferencial = entidade.DiaPagamentoPreferencial,
                Ativo = entidade.Ativo,
                PresencaMensal = entidade.Presencas.Count(p => p.Aula.DataAula.Month == System.DateTime.Now.Month && p.Aula.DataAula.Year == System.DateTime.Now.Year && p.Presente),
                TipoContatoDisplay = entidade.TipoContato.GetDisplayName()
            };
        }

        public static List<AlunoViewModel> ToViewModel(List<Aluno> entidades, int mesFiltro)
        {
            if (entidades == null) return new List<AlunoViewModel>();

            return entidades.Select(entidade => ToViewModel(entidade, mesFiltro)).ToList();
        }
    }
}