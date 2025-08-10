using Atelie.Core.Enums;

namespace Atelie.Web.ViewModels.Entities
{
    public class AlunoViewModel
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public TipoContato TipoContato { get; set; }
        public string DescricaoContato { get; set; }
        public int DiaPagamentoPreferencial { get; set; }
        public bool Ativo { get; set; }
        public int PresencaMensal { get; set; }
        public string TipoContatoDisplay { get; set; }
    }
}