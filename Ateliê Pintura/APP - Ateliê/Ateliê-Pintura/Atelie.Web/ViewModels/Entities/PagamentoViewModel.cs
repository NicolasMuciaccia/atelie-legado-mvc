using Atelie.Core.Enums;
using System;

namespace Atelie.Web.ViewModels.Entities
{
    public class PagamentoViewModel
    {
        public long Id { get; set; }
        public DateTime DataReferencia { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime? DataEfetivacao { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        public string TipoPagamentoDisplay { get; set; }
        public bool Pendente { get; set; }
        public long AlunoId { get; set; }
        public string AlunoNome { get; set; }
        public long? TurmaId { get; set; }
        public string TurmaNome { get; set; }
        public string PagamentoNome { get; set; }
    }
}