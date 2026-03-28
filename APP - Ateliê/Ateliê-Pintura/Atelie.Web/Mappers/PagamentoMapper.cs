using Atelie.Core.Utils;
using Atelie.Domain.Entities;
using Atelie.Web.ViewModels.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Atelie.Web.Mappers
{
    public static class PagamentoMapper
    {
        public static PagamentoViewModel ToViewModel(Pagamento entidade)
        {
            if (entidade == null) return null;

            return new PagamentoViewModel
            {
                Id = entidade.Id,
                DataReferencia = entidade.DataReferencia,
                ValorPago = entidade.ValorPago,
                DataEfetivacao = entidade.DataEfetivacao,
                TipoPagamento = entidade.TipoPagamento,
                TipoPagamentoDisplay = entidade.TipoPagamento.GetDisplayName(),
                Pendente = entidade.Pendente,
                AlunoId = entidade.Aluno?.Id ?? 0,
                AlunoNome = entidade.Aluno?.Nome,
                TurmaId = entidade.Turma?.Id ?? 0,
                TurmaNome = entidade.Turma?.GetNomeTurma(),
                PagamentoNome = entidade.GetPagamentoNome()
            };
        }

        public static List<PagamentoViewModel> ToViewModel(List<Pagamento> entidades)
        {
            if (entidades == null) return new List<PagamentoViewModel>();

            return entidades.Select(ToViewModel).ToList();
        }
    }
}