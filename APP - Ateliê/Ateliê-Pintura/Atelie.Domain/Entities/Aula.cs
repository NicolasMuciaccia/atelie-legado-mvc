using System;
using System.Collections.Generic;
using Atelie.Core.Entities;

namespace Atelie.Domain.Entities
{
    public class Aula : AuditableEntity
    {
        public virtual DateTime DataAula { get; set; }

        public virtual Turma Turma { get; set; }
        public virtual IList<Presenca> Presencas { get; set; } = new List<Presenca>();

        public Aula() { }

        public virtual string GetAulaNome()
        {
            return $"{DataAula.ToString("dd/MM/yyyy")}";
        }
    }
}
