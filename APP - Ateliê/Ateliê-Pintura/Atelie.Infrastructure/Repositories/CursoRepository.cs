using System;
using System.Linq;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Repositories;
using NHibernate;

namespace Atelie.Infrastructure.Repositories
{
    public class CursoRepository : NHibernateRepository<Curso>, ICursoRepository
    {
        public CursoRepository(ISession session) : base(session) { }

        public int SwitchAtivo(Curso curso)
        {
            var hql = @"
            UPDATE Curso c
            SET c.Ativo = :ativa,
                c.AtualizadoEm = :data,
                c.AtualizadoPor = :usuario
            WHERE c.Id = :id";

            var query = _session.CreateQuery(hql);

            if (curso.Ativo is true)
                query.SetParameter("ativa", false);
            else
                query.SetParameter("ativa", true);

            query.SetParameter("data", DateTime.UtcNow);
            query.SetParameter("usuario", (long)1974);
            query.SetParameter("id", curso.Id);

            return query.ExecuteUpdate();
        }

        public bool NomeDeCursoExiste(Curso curso)
        {
            if(curso.Nome != null)
            {
                return _session.Query<Curso>().Any(c => c.Id != curso.Id && c.Nome.ToLower() == curso.Nome.ToLower());
            }
            else
            {
                return false;
            }
        }
    }
}