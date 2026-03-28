using System.Collections.Generic;
using System;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Repositories;
using NHibernate;
using System.Linq;
using Atelie.Infrastructure.Services.DTOs;
using Atelie.Core.Enums;
using System.Runtime.Remoting.Contexts;

namespace Atelie.Infrastructure.Repositories
{
    public class TurmaRepository : NHibernateRepository<Turma>, ITurmaRepository
    {
        public TurmaRepository(ISession session) : base(session) { }

        public List<Turma> ListarTodosComFiltro(TurmaFiltroDTO filtro)
        {
            var query = _session.Query<Turma>();

            if (filtro.CursoId.HasValue && filtro.CursoId > 0)
                query = query.Where(t => t.Curso.Id == filtro.CursoId.Value);

            if (filtro.DiaDaSemana.HasValue)
            {
                var diaDaSemana = (DiaDaSemana)filtro.DiaDaSemana.Value;
                query = query.Where(t => t.DiaDaSemana == diaDaSemana);
            }

            if (filtro.Ativo.HasValue)
                query = query.Where(t => t.Ativo == filtro.Ativo.Value);

            return query.ToList();
        }

        public List<long> ObterTurmaIdPorCursoId(long id)
        {
            return _session.Query<Turma>()
                .Where(t => t.Curso.Id == id && t.Ativo)
                .Select(t => t.Id)
                .ToList();
        }

        public List<string> ObterNomesDeCursosAtivosPorCursoId(long cursoId)
        {
            return _session.Query<Turma>()
                .Where(t => t.Ativo && t.Curso.Id == cursoId)
                .Select(t => t.Curso.Nome)
                .Distinct()
                .ToList();
        }

        public int DesativarEmLote(List<long> ids)
        {
            if (ids == null || !ids.Any())
                return 0;

            var hql = @"
            UPDATE Turma t
            SET t.Ativo = :ativa,
                t.AtualizadoEm = :data,
                t.AtualizadoPor = :usuario
            WHERE t.Id IN (:ids)";

            var query = _session.CreateQuery(hql);
            query.SetParameter("ativa", false);
            query.SetParameter("data", DateTime.UtcNow);
            query.SetParameter("usuario", (long)1974);
            query.SetParameterList("ids", ids);

            return query.ExecuteUpdate();
        }

        public int DesativarTurmaAlunosEmLote(List<long> ids)
        {
            var sql = "DELETE FROM public.turmas_alunos WHERE id_turma IN (:ids)";

            var query = _session.CreateSQLQuery(sql);
            query.SetParameterList("ids", ids);

            return query.ExecuteUpdate();
        }


        public bool ExisteTurmasRepetidas(Turma turma)
        {
            if (turma.Horario == TimeSpan.Zero)
                return false;

            var inicioIntervalo = turma.Horario - TimeSpan.FromMinutes(30);
            var fimIntervalo = turma.Horario + TimeSpan.FromMinutes(30);

            var turmasConflitantes = _session.Query<Turma>()
                .Where(t => t.Id != turma.Id
                    && t.DiaDaSemana == turma.DiaDaSemana
                    && t.Horario >= inicioIntervalo
                    && t.Horario <= fimIntervalo);

            return turmasConflitantes.Any();
        }

        public List<Turma> ObterTurmaPorAlunoId(long id)
        {
            var turmas = _session.Query<Turma>()
                .Where(t => t.Alunos.Any(a => a.Id == id))
                .ToList();

            return turmas;
        }
    }
}