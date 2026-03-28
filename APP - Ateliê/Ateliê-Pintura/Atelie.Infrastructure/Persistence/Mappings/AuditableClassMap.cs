using Atelie.Core.Entities;
using FluentNHibernate.Mapping;

namespace Atelie.Infrastructure.Persistence.Mappings
{
    public abstract class AuditableClassMap<T> : ClassMap<T> where T : AuditableEntity
    {
        public AuditableClassMap(string tableName, string idColumnName, string sequenceName)
        {
            Table(tableName);
            Id(x => x.Id).Column(idColumnName).GeneratedBy.Sequence(sequenceName);
            
            Map(x => x.CriadoEm).Column("DT_CRIADO_EM").Not.Nullable().Not.Update();
            Map(x => x.CriadoPor).Column("ID_CRIADO_POR").Not.Nullable().Not.Update();
            Map(x => x.AtualizadoEm).Column("DT_ATUALIZADO_EM").Not.Nullable();
            Map(x => x.AtualizadoPor).Column("ID_ATUALIZADO_POR").Not.Nullable();
        }
    }
}
