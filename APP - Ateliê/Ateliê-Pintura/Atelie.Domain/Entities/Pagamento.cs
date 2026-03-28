using System;
using Atelie.Core.Entities;
using Atelie.Core.Enums;

namespace Atelie.Domain.Entities
{
    public class Pagamento : AuditableEntity
    {
        public virtual DateTime DataReferencia { get; set; }
        public virtual decimal ValorPago { get; set; }
        public virtual DateTime? DataEfetivacao { get; set; }
        public virtual TipoPagamento TipoPagamento { get; set; }
        public virtual bool Pendente { get; set; }

        public virtual Aluno Aluno { get; set; }
        public virtual Turma Turma { get; set; }

        public Pagamento()
        {
            this.Pendente = true;
        }

        public virtual string GetPagamentoNome()
        {
            return $"{Aluno?.Nome}-{DataReferencia:MM/yyyy}-{ValorPago:C}";
        }
    }
}
