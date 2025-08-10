using System;
using System.Collections.Generic;

namespace Atelie.Web.ViewModels.Entities
{
    public class AulaViewModel
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataAula { get; set; }
        public long TurmaId { get; set; }
        public string TurmaNome { get; set; }
        public bool TurmaAtiva { get; set; }
        public long TotalPresencas { get; set; }
        public long TotalFaltas { get; set; }
        public List<PresencaViewModel> Presencas { get; set; } = new List<PresencaViewModel>();
    }
}