using System.Collections.Generic;
using System.Linq;
using Atelie.Domain.Entities.Views;
using Atelie.Infrastructure.Abstractions.Repositories;
using Atelie.Infrastructure.Services.DTOs;
using NHibernate;

namespace Atelie.Infrastructure.Repositories
{
    public class ViewRelatorioPagamentoRepository : IViewRelatorioPagamentoRepository
    {
        protected readonly ISession _session;

        public ViewRelatorioPagamentoRepository(ISession session)
        {
            _session = session;
        }

        public List<ViewRelatorioPagamento> ListarTodosComFiltro(ViewRelatorioPagamentoFiltroDTO filtro)
        {
            var query = _session.Query<ViewRelatorioPagamento>();

            if (filtro == null)
                return query.ToList();

            if (filtro.MesFiltro > 0 && filtro.MesFiltro <= 12)
                query = query.Where(p => p.DataReferencia.Month == filtro.MesFiltro);

            if (filtro.AnoFiltro > 0)
                query = query.Where(p => p.DataReferencia.Year == filtro.AnoFiltro);

            if (filtro.PendenteFiltro.HasValue)
                query = query.Where(p => p.Pendente == filtro.PendenteFiltro.Value);

            return query.ToList();
        }
    }
}
