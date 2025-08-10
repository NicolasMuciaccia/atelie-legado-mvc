using System.Collections.Generic;

namespace Atelie.Core.Utils
{
    public static class ValorPagamentoSelectUtil
    {
        public static readonly Dictionary<decimal, string> Lista = new Dictionary<decimal, string>
        {
            { 100.00m, "R$ 100,00" },
            { 120.00m, "R$ 120,00" },
            { 150.00m, "R$ 150,00" }
        };
    }
}
