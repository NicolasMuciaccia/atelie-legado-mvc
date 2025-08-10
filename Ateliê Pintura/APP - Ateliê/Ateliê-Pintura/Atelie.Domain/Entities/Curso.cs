using System.Collections.Generic;
using Atelie.Core.Entities;

namespace Atelie.Domain.Entities
{
    public class Curso : AuditableEntity
    {
        public virtual string Nome { get; set; }
        public virtual decimal ValorMensal { get; set; }
        public virtual bool Ativo { get; set; }

        public virtual IList<Turma> Turmas { get; set; } = new List<Turma>();

        public Curso() { }
    }
}
