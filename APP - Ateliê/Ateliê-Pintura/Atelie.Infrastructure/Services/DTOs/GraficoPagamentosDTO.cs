using System.Collections.Generic;

namespace Atelie.Infrastructure.Services.DTOs
{
    public class GraficoPagamentosDTO
    {
        public List<string> Labels { get; set; }
        public List<decimal> ValoresPagos { get; set; }
        public List<decimal> ValoresPendentes { get; set; }
    }
}
