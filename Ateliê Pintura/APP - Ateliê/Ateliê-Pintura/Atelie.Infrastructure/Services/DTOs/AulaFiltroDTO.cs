using System;

namespace Atelie.Infrastructure.Services.DTOs
{
    public class AulaFiltroDTO
    {
        public long? TurmaId { get; set; }
        public DateTime? DataDaAulaDe { get; set; }
        public DateTime? DataDaAulaAte { get; set; }
        public bool? AtivoFiltro { get; set; }
    }
}
