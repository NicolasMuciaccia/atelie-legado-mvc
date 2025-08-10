using Atelie.Core.Enums;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Persistence.Mappings;
using NHibernate.Type;

public class TurmaMap : AuditableClassMap<Turma>
{
    public TurmaMap() : base("TURMAS", "ID_TURMA", "SEQ_TURMAS")
    {
        Map(x => x.DiaDaSemana).Column("TP_DIA_SEMANA").CustomType<EnumType<DiaDaSemana>>().Not.Nullable();
        Map(x => x.Horario).Column("HR_AULA").CustomType<TimeAsTimeSpanType>().Not.Nullable();
        Map(x => x.Ativo).Column("FL_ATIVA").Not.Nullable();

        References(x => x.Curso).Column("ID_CURSO").Not.Nullable().Cascade.None();
        HasMany(x => x.Aulas).KeyColumn("ID_TURMA").Inverse().Cascade.SaveUpdate();
        HasMany(x => x.Pagamentos).KeyColumn("ID_TURMA").Inverse().Cascade.SaveUpdate();
        HasManyToMany(x => x.Alunos).Table("TURMAS_ALUNOS").ParentKeyColumn("ID_TURMA").ChildKeyColumn("ID_ALUNO").Cascade.SaveUpdate();
    }
}