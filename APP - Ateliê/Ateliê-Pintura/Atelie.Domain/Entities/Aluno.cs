using System.Collections.Generic;
using Atelie.Core.Entities;
using Atelie.Core.Enums;

namespace Atelie.Domain.Entities
{
    public class Aluno : AuditableEntity
    {
        public virtual string Nome { get; set; }
        public virtual TipoContato TipoContato { get; set; }
        public virtual string DescricaoContato { get; set; }
        public virtual int DiaPagamentoPreferencial { get; set; }
        public virtual bool Ativo { get; set; }

        public virtual IList<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
        public virtual IList<Presenca> Presencas { get; set; } = new List<Presenca>();
        public virtual IList<Turma> Turmas { get; set; } = new List<Turma>();

        public Aluno() { }
    }
}
