using System;
namespace Atelie.Domain.Entities.Views
{
    public class ViewRelatorioPagamento
    {
        public virtual long IdPagamento { get; protected set; }

        public virtual long IdAluno { get; protected set; }
        public virtual long? IdCurso { get; protected set; }
        public virtual long? IdTurma { get; protected set; }
        public virtual string NomeAluno { get; protected set; }
        public virtual string TipoContatoAluno { get; protected set; }
        public virtual string Contato { get; protected set; }
        public virtual DateTime DataReferencia { get; protected set; }
        public virtual decimal ValorPago { get; protected set; }
        public virtual bool Pendente { get; protected set; }

        public virtual DateTime? DataEfetivacao { get; protected set; }
        public virtual string TipoPagamento { get; protected set; }
        public virtual string NomeCurso { get; protected set; }
        public virtual string DiaSemana { get; protected set; }

        public virtual TimeSpan? HoraAula { get; protected set; }
        public virtual decimal? ValorMensalCurso { get; protected set; }
    }
}
