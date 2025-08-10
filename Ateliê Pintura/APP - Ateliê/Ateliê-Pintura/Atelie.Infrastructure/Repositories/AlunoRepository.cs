using System.Collections.Generic;
using System.Linq;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Repositories;
using Atelie.Infrastructure.Services.DTOs;
using NHibernate;

namespace Atelie.Infrastructure.Repositories
{
    public class AlunoRepository : NHibernateRepository<Aluno>, IAlunoRepository
    {
        public AlunoRepository(ISession session) : base(session) { }

        public List<Aluno> ListarTodosComFiltro(AlunoFiltroDTO filtro)
        {
            var query = _session.Query<Aluno>();

            if (!string.IsNullOrWhiteSpace(filtro.NomeFiltro))
                query = query.Where(a => a.Nome.ToLower().Contains(filtro.NomeFiltro.ToLower()));

            if (filtro.DiaPagamentoPreferencialFiltro > 0 && filtro.DiaPagamentoPreferencialFiltro <= 31)
                query = query.Where(a => a.DiaPagamentoPreferencial == filtro.DiaPagamentoPreferencialFiltro);

            if (filtro.AtivoFiltro.HasValue)
                query = query.Where(a => a.Ativo == filtro.AtivoFiltro.Value);

            return query.ToList();
        }

        public int DesativarTurmaAlunos(long id)
        {
            var sql = "DELETE FROM public.turmas_alunos WHERE id_aluno = :id";

            var query = _session.CreateSQLQuery(sql);
            query.SetParameter("id", id);

            return query.ExecuteUpdate();
        }

        public bool NomeDeAlunoExiste(Aluno aluno)
        {
            if(aluno.Nome !=null)
                return _session.Query<Aluno>().Any(a => a.Id != aluno.Id && a.Nome.ToLower() == aluno.Nome.ToLower());

            return false;
        }
    }
}