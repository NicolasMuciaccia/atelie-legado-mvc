using Atelie.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace Atelie.Core.Enums
{
    public enum TipoPagamento
    {
        [Display(Name = "Aguardando", ResourceType = typeof(GlobalMessages))]
        Aguardando = 0,

        [Display(Name = "Credito", ResourceType = typeof(GlobalMessages))]
        Credito = 1,

        [Display(Name = "Debito", ResourceType = typeof(GlobalMessages))]
        Debito = 2,

        [Display(Name = "Pix", ResourceType = typeof(GlobalMessages))]
        Pix = 3,

        [Display(Name = "Dinheiro", ResourceType = typeof(GlobalMessages))]
        Dinheiro = 4,
    }
}
