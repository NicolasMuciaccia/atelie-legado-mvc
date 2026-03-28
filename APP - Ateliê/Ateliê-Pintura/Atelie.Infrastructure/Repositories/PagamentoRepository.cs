using System.Collections.Generic;
using System.Linq;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Repositories;
using Atelie.Infrastructure.Services.DTOs;
using NHibernate;

namespace Atelie.Infrastructure.Repositories
{
    public class PagamentoRepository : NHibernateRepository<Pagamento>, IPagamentoRepository
    {
        public PagamentoRepository(ISession session) : base(session) { }

        public List<Pagamento> ListarTodosComFiltro(PagamentoFiltroDTO filtro)
        {
            var query = _session.Query<Pagamento>();

            if (!string.IsNullOrWhiteSpace(filtro.AlunoNomeFiltro))
                query = query.Where(p => p.Aluno.Nome.ToLower().Contains(filtro.AlunoNomeFiltro.ToLower()));

            if (filtro.MesFiltro > 0 && filtro.MesFiltro <= 12)
                query = query.Where(p => p.DataReferencia.Month == filtro.MesFiltro);

            if (filtro.AnoFiltro > 0)
                query = query.Where(p => p.DataReferencia.Year == filtro.AnoFiltro);

            if (filtro.PendenteFiltro.HasValue)
                query = query.Where(p => p.Pendente == filtro.PendenteFiltro.Value);

            if (filtro.TipoPagamentoFiltro.HasValue)
                query = query.Where(p => p.TipoPagamento == filtro.TipoPagamentoFiltro.Value);

            return query.ToList();
        }

        public bool ExisteOutroPagamentoNoMesmoMes(Pagamento pagamento)
        {
            return GetAll().Any(p =>
                p.Id != pagamento.Id &&
                p.Aluno.Id == pagamento.Aluno.Id &&
                p.Turma.Id == pagamento.Turma.Id &&
                p.DataReferencia.Month == pagamento.DataReferencia.Month &&
                p.DataReferencia.Year == pagamento.DataReferencia.Year);
        }

    }
}