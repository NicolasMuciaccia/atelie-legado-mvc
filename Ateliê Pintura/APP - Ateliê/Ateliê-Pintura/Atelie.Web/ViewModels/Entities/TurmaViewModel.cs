using Atelie.Core.Enums;
using System;
using System.Collections.Generic;

namespace Atelie.Web.ViewModels.Entities
{
    public class TurmaViewModel
    {
        public long Id { get; set; }
        public string NomeCompleto { get; set; }
        public DiaDaSemana DiaDaSemana { get; set; }
        public string DiaDaSemanaDisplay { get; set; }
        public TimeSpan Horario { get; set; }
        public bool Ativo { get; set; }
        public long CursoId { get; set; }
        public string CursoNome { get; set; }
        public int TotalAlunos { get; set; }
        public List<AlunoSelectListViewModel> Alunos { get; set; } = new List<AlunoSelectListViewModel>();
    }
}