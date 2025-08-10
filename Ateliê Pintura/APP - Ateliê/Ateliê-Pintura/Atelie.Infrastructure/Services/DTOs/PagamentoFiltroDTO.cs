using Atelie.Core.Enums;

namespace Atelie.Infrastructure.Services.DTOs
{
    public class PagamentoFiltroDTO
    {
        public string AlunoNomeFiltro { get; set; }
        public int? MesFiltro { get; set; }
        public int? AnoFiltro { get; set; }
        public bool? PendenteFiltro { get; set; }
        public TipoPagamento? TipoPagamentoFiltro { get; set; }
    }
}
