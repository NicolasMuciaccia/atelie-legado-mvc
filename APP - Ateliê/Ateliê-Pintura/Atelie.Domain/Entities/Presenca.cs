using Atelie.Core.Entities;

namespace Atelie.Domain.Entities
{
    public class Presenca : AuditableEntity
    {
        public virtual bool Presente { get; set; }

        public virtual Aluno Aluno { get; set; }
        public virtual Aula Aula { get; set; }

        public Presenca()
        {
            this.Presente = false;
        }

        public virtual string GetPresencaNome()
        {
            return $"{Aluno?.Nome} - {Aula?.GetAulaNome()}";
        }
    }
}
