using Atelie.Core.Enums;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Persistence.Mappings;
using NHibernate.Type;

public class PagamentoMap : AuditableClassMap<Pagamento>
{
    public PagamentoMap() : base("PAGAMENTOS", "ID_PAGAMENTO", "SEQ_PAGAMENTOS")
    {
        Map(x => x.DataReferencia, "DT_REFERENCIA").Column("DT_REFERENCIA").Not.Nullable();
        Map(x => x.ValorPago).Column("VL_PAGO").Not.Nullable();
        Map(x => x.DataEfetivacao).Column("DT_EFETIVACAO").Nullable();
        Map(x => x.TipoPagamento).Column("TP_PAGAMENTO").CustomType<EnumType<TipoPagamento>>().Not.Nullable();
        Map(x => x.Pendente).Column("FL_PENDENTE").Not.Nullable();

        References(x => x.Aluno).Column("ID_ALUNO").Not.Nullable().Cascade.None();
        References(x => x.Turma).Column("ID_TURMA").Cascade.None();
    }
}