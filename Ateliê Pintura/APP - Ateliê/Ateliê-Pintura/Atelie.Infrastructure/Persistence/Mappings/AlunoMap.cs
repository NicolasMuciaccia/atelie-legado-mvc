using Atelie.Core.Enums;
using Atelie.Core.Utils;
using Atelie.Domain.Entities;
using NHibernate.Type;

namespace Atelie.Infrastructure.Persistence.Mappings
{
    public class AlunoMap : AuditableClassMap<Aluno>
    {
        public AlunoMap() : base("ALUNOS", "ID_ALUNO", "SEQ_ALUNOS")
        {
            Map(x => x.Nome).Column("NM_ALUNO").Length(DatabaseLengthDefinitions.Nome).Not.Nullable();
            Map(x => x.TipoContato).Column("TP_CONTATO").CustomType<EnumType<TipoContato>>().Not.Nullable();
            Map(x => x.DescricaoContato).Column("DS_CONTATO").Length(DatabaseLengthDefinitions.Descricao).Not.Nullable();
            Map(x => x.DiaPagamentoPreferencial).Column("NR_DIA_PAGAMENTO").Not.Nullable();
            Map(x => x.Ativo).Column("FL_ATIVA").Not.Nullable();

            HasMany(x => x.Pagamentos).KeyColumn("ID_ALUNO").Inverse().Cascade.SaveUpdate();
            HasMany(x => x.Presencas).KeyColumn("ID_ALUNO").Inverse().Cascade.SaveUpdate();
            HasManyToMany(x => x.Turmas).Table("TURMAS_ALUNOS").ParentKeyColumn("ID_ALUNO").ChildKeyColumn("ID_TURMA").Inverse().Cascade.SaveUpdate();
        }
    }
}
