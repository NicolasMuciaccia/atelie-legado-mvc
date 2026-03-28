using System.Collections.Generic;
using System.Linq;
using Atelie.Domain.Entities.Views;
using Atelie.Infrastructure.Abstractions.Repositories;
using Atelie.Infrastructure.Abstractions.Services;
using Atelie.Infrastructure.Services.DTOs;

namespace Atelie.Infrastructure.Services
{
    public class ViewRelatorioPagamentoService : IViewRelatorioPagamentoService
    {
        private readonly IViewRelatorioPagamentoRepository _viewRelatorioRepository;

        public ViewRelatorioPagamentoService(IViewRelatorioPagamentoRepository viewRelatorioRepository)
        {
            _viewRelatorioRepository = viewRelatorioRepository;
        }

        public List<ViewRelatorioPagamento> ListarTodosComFiltro(ViewRelatorioPagamentoFiltroDTO filtro)
        {
            return _viewRelatorioRepository.ListarTodosComFiltro(filtro);
        }

        public GraficoPagamentosDTO ObterDadosGrafico(ViewRelatorioPagamentoFiltroDTO filtro)
        {
            var filtroParaGrafico = new ViewRelatorioPagamentoFiltroDTO
            {
                AnoFiltro = filtro?.AnoFiltro,
                PendenteFiltro = filtro?.PendenteFiltro
            };
            var pagamentosDoAno = _viewRelatorioRepository.ListarTodosComFiltro(filtroParaGrafico);

            var resultado = new GraficoPagamentosDTO
            {
                Labels = new List<string> { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" },
                ValoresPagos = new List<decimal>(new decimal[12]),
                ValoresPendentes = new List<decimal>(new decimal[12])
            };

            var dadosAgrupados = pagamentosDoAno
                .GroupBy(p => p.DataReferencia.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    TotalPago = g.Where(p => !p.Pendente).Sum(p => p.ValorPago),
                    TotalPendente = g.Where(p => p.Pendente).Sum(p => p.ValorMensalCurso ?? 0)
                })
                .ToList();

            foreach (var item in dadosAgrupados)
            {
                resultado.ValoresPagos[item.Mes - 1] = item.TotalPago;
                resultado.ValoresPendentes[item.Mes - 1] = item.TotalPendente;
            }

            return resultado;
        }
    }
}
