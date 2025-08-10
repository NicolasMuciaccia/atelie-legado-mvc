using Atelie.Domain.Entities.Views;
using FluentNHibernate.Mapping;

namespace Atelie.Infrastructure.Persistence.Mappings.Views
{
    public class ViewRelatorioPagamentoMap : ClassMap<ViewRelatorioPagamento>
    {
        public ViewRelatorioPagamentoMap()
        {
            Table("VW_RELATORIO_PAGAMENTOS");
            Schema("public");
            ReadOnly();

            Id(x => x.IdPagamento).Column("ID_PAGAMENTO").GeneratedBy.Assigned();

            Map(x => x.IdAluno).Column("ID_ALUNO").Not.Nullable();
            Map(x => x.IdCurso).Column("ID_CURSO");
            Map(x => x.IdTurma).Column("ID_TURMA");
            Map(x => x.NomeAluno).Column("NM_ALUNO").Not.Nullable();
            Map(x => x.TipoContatoAluno).Column("DS_TP_CONTATO_ALUNO").Not.Nullable();
            Map(x => x.Contato).Column("DS_CONTATO").Not.Nullable();
            Map(x => x.DataReferencia).Column("DT_REFERENCIA").Not.Nullable();
            Map(x => x.ValorPago).Column("VL_PAGO").Not.Nullable();
            Map(x => x.Pendente).Column("FL_PENDENTE").Not.Nullable();
            Map(x => x.DataEfetivacao).Column("DT_EFETIVACAO");
            Map(x => x.TipoPagamento).Column("DS_TP_PAGAMENTO").Not.Nullable();
            Map(x => x.NomeCurso).Column("NM_CURSO");
            Map(x => x.DiaSemana).Column("DS_DIA_SEMANA");
            Map(x => x.HoraAula).Column("HR_AULA").CustomType("TimeAsTimeSpan");
            Map(x => x.ValorMensalCurso).Column("VL_MENSAL_CURSO");
        }
    }
}
