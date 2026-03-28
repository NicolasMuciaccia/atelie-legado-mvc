using System.ComponentModel.DataAnnotations;
using Atelie.Core.Resources;

namespace Atelie.Core.Enums
{
    public enum DiaDaSemana
    {
        [Display(Name = "Domingo", ResourceType = typeof(GlobalMessages))]
        Domingo = 1,

        [Display(Name = "Segunda", ResourceType = typeof(GlobalMessages))]
        Segunda = 2,

        [Display(Name = "Terca", ResourceType = typeof(GlobalMessages))]
        Terca = 3,

        [Display(Name = "Quarta", ResourceType = typeof(GlobalMessages))]
        Quarta = 4,

        [Display(Name = "Quinta", ResourceType = typeof(GlobalMessages))]
        Quinta = 5,

        [Display(Name = "Sexta", ResourceType = typeof(GlobalMessages))]
        Sexta = 6,

        [Display(Name = "Sabado", ResourceType = typeof(GlobalMessages))]
        Sabado = 7
    }
}
