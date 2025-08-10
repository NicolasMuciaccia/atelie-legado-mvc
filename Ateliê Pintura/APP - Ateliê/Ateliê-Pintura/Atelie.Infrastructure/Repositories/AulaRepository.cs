using System;
using System.Collections.Generic;
using System.Linq;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Repositories;
using Atelie.Infrastructure.Services.DTOs;
using NHibernate;

namespace Atelie.Infrastructure.Repositories
{
    public class AulaRepository : NHibernateRepository<Aula>, IAulaRepository
    {
        public AulaRepository(ISession session) : base(session) { }

        public List<Aula> ListarTodosComFiltro(AulaFiltroDTO filtro)
        {
            var query = _session.Query<Aula>();

            if (filtro.TurmaId.HasValue && filtro.TurmaId.Value > 0)
                query = query.Where(a => a.Turma.Id == filtro.TurmaId);

            if (filtro.DataDaAulaDe.HasValue)
                query = query.Where(a => a.DataAula >= filtro.DataDaAulaDe.Value);

            if (filtro.DataDaAulaAte.HasValue)
                query = query.Where(a => a.DataAula <= filtro.DataDaAulaAte.Value);

            if (filtro.AtivoFiltro.HasValue)
                query = query.Where(a => a.Turma.Ativo == filtro.AtivoFiltro.Value);

            return query.ToList();
        }

    }
}