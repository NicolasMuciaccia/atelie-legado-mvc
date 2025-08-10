using Atelie.Domain.Entities;
using Atelie.Infrastructure.Persistence.Mappings;

public class PresencaMap : AuditableClassMap<Presenca>
{
    public PresencaMap() : base("PRESENCAS", "ID_PRESENCA", "SEQ_PRESENCAS")
    {
        Map(x => x.Presente).Column("FL_PRESENTE").Not.Nullable();

        References(x => x.Aluno).Column("ID_ALUNO").Not.Nullable().Cascade.None();
        References(x => x.Aula).Column("ID_AULA").Not.Nullable().Cascade.None();
    }
}