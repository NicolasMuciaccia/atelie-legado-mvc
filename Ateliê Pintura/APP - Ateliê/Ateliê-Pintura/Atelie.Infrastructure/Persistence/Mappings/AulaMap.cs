using Atelie.Core.Utils;
using Atelie.Domain.Entities;

namespace Atelie.Infrastructure.Persistence.Mappings
{
    public class AulaMap : AuditableClassMap<Aula>
    {
        public AulaMap() : base("AULAS", "ID_AULA", "SEQ_AULAS")
        {
            Map(x => x.DataAula).Column("DT_AULA").Not.Nullable();

            References(x => x.Turma).Column("ID_TURMA").Cascade.None();
            HasMany(x => x.Presencas).KeyColumn("ID_AULA").Inverse().Cascade.SaveUpdate();
        }
    }
}
