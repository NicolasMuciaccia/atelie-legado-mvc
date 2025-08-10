using Atelie.Core.Resources;
using System.ComponentModel.DataAnnotations;

namespace Atelie.Core.Enums
{
    public enum TipoContato
    {
        [Display(Name = "NaoInformado", ResourceType = typeof(GlobalMessages))]
        NaoInformado = 0,

        [Display(Name = "Telefone", ResourceType = typeof(GlobalMessages))]
        Telefone = 1,

        [Display(Name = "Celular", ResourceType = typeof(GlobalMessages))]
        Celular = 2,

        [Display(Name = "Email", ResourceType = typeof(GlobalMessages))]
        Email = 3,
    }
}
