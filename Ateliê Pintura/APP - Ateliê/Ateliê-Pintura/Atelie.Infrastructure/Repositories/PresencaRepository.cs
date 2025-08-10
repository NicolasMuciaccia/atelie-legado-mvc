using System.Collections.Generic;
using System.Linq;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Repositories;
using NHibernate;

namespace Atelie.Infrastructure.Repositories
{
    public class PresencaRepository : NHibernateRepository<Presenca>, IPresencaRepository
    {
        public PresencaRepository(ISession session) : base(session) { }

        public List<Presenca> ListarPorAulaId(long aulaId)
        {
            return _session.Query<Presenca>()
                .Where(p => p.Aula.Id == aulaId)
                .ToList();
        }

        public bool ExisteOutraPresencaDoAlunoNaAula(Presenca presenca)
        {
            return _session.Query<Presenca>()
                .Any(p =>
                p.Id != presenca.Id &&
                p.Aula.Id == presenca.Aula.Id &&
                p.Aluno.Id == presenca.Aluno.Id);
        }

    }
}