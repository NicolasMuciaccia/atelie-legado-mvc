using System;
using System.Collections.Generic;
using Atelie.Core.Entities;
using Atelie.Core.Enums;
using Atelie.Core.Utils;

namespace Atelie.Domain.Entities
{
    public class Turma : AuditableEntity
    {
        public virtual DiaDaSemana DiaDaSemana { get; set; }
        public virtual TimeSpan Horario { get; set; }
        public virtual bool Ativo { get; set; }

        public virtual Curso Curso { get; set; }
        public virtual IList<Aula> Aulas { get; set; } = new List<Aula>();
        public virtual IList<Aluno> Alunos { get; set; } = new List<Aluno>();
        public virtual IList<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();

        public Turma() { }

        public virtual string GetNomeTurma()
        {
            return $"{Curso?.Nome} - {DiaDaSemana.GetDisplayName()} às {Horario:hh\\:mm}";
        }
    }
}
