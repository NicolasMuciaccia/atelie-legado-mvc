using Atelie.Core.Utils;
using Atelie.Domain.Entities;

namespace Atelie.Infrastructure.Persistence.Mappings
{
    public class CursoMap : AuditableClassMap<Curso>
    {
        public CursoMap() : base("CURSOS", "ID_CURSO", "SEQ_CURSOS")
        {
            Map(x => x.Nome).Column("NM_CURSO").Length(DatabaseLengthDefinitions.Nome).Not.Nullable();
            Map(x => x.ValorMensal).Column("VL_MENSAL").Not.Nullable();
            Map(x => x.Ativo).Column("FL_ATIVA").Not.Nullable();

            HasMany(x => x.Turmas).KeyColumn("ID_CURSO").Inverse().Cascade.SaveUpdate();
        }
    }
}
