using Atelie.Core.Enums;

namespace Atelie.Infrastructure.Services.DTOs
{
    public class AlunoFiltroDTO
    {
        public string NomeFiltro { get; set; }
        public int DiaPagamentoPreferencialFiltro { get; set; }
        public bool? AtivoFiltro { get; set; }
        public int MesDasPresencasFiltro { get; set; }
    }
}
