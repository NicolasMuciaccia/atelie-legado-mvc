namespace Atelie.Web.ViewModels.Entities
{
    public class PresencaViewModel
    {
        public long Id { get; set; }
        public long AlunoId { get; set; }
        public string AlunoNome { get; set; }
        public bool Presente { get; set; }
    }
}