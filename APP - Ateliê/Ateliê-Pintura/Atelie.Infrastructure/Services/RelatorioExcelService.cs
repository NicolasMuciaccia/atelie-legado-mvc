using Atelie.Domain.Entities.Views;
using Atelie.Infrastructure.Abstractions.Services;
using Atelie.Infrastructure.Services.DTOs;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Collections.Generic;
using System;
using System.Linq;

public class RelatorioExcelService : IRelatorioExcelService
{
    private readonly IViewRelatorioPagamentoService _viewRelatorioService;

    public RelatorioExcelService(IViewRelatorioPagamentoService viewRelatorioService)
    {
        _viewRelatorioService = viewRelatorioService;
    }


    public string FormatarNomeExcel(ViewRelatorioPagamentoFiltroDTO filtro)
    {
        string anoFiltro = filtro?.AnoFiltro?.ToString() ?? "XXXX";
        string mesFiltro = filtro?.MesFiltro?.ToString("D2") ?? "XX";

        string statusFiltro;
        if (filtro == null || !filtro.PendenteFiltro.HasValue)
        {
            statusFiltro = "Todos";
        }
        else
        {
            statusFiltro = filtro.PendenteFiltro.Value ? "Pendentes" : "Pagos";
        }

        string dataImpressao = DateTime.Now.ToString("yyyyMMdd");

        return $"RelatorioPagamentos_({mesFiltro}-{anoFiltro}-{statusFiltro})_{dataImpressao}.xlsx";
    }

    public byte[] GerarPlanilhaExcel(ViewRelatorioPagamentoFiltroDTO filtro)
    {
        List<ViewRelatorioPagamento> dados = _viewRelatorioService.ListarTodosComFiltro(filtro);

        dados = dados.OrderByDescending(d => d.DataReferencia).ThenBy(d => d.NomeCurso).ToList();

        using (var package = new ExcelPackage())
        {
            var planilha = package.Workbook.Worksheets.Add("Relatório de Pagamentos");

            string[] headers = {
                "Aluno", "Contato", "Curso", "Dia da Aula", "Horário", "Pendente?", "Valor do Curso", "Data Ref.", "Valor Pago",
                "Data Pag.", "Forma Pag.",
                "ID Pag.", "ID Aluno", "ID Curso", "ID Turma"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                planilha.Cells[1, i + 1].Value = headers[i];
            }

            using (var range = planilha.Cells[1, 1, 1, headers.Length])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            if (dados.Any())
            {
                for (int i = 0; i < dados.Count; i++)
                {
                    var item = dados[i];
                    int row = i + 2;

                    planilha.Cells[row, 1].Value = item.NomeAluno;
                    planilha.Cells[row, 2].Value = $"{item.TipoContatoAluno}: {item.Contato}";
                    planilha.Cells[row, 3].Value = item.NomeCurso;
                    planilha.Cells[row, 4].Value = item.DiaSemana;
                    if (item.HoraAula.HasValue) { planilha.Cells[row, 5].Value = item.HoraAula.Value; }
                    planilha.Cells[row, 6].Value = item.Pendente ? "Sim" : "Não";
                    planilha.Cells[row, 7].Value = item.ValorMensalCurso;
                    planilha.Cells[row, 8].Value = item.DataReferencia;
                    planilha.Cells[row, 9].Value = item.ValorPago;
                    if (item.DataEfetivacao.HasValue) { planilha.Cells[row, 10].Value = item.DataEfetivacao.Value; }
                    planilha.Cells[row, 11].Value = item.TipoPagamento;

                    planilha.Cells[row, 12].Value = item.IdPagamento;
                    planilha.Cells[row, 13].Value = item.IdAluno;
                    planilha.Cells[row, 14].Value = item.IdCurso;
                    planilha.Cells[row, 15].Value = item.IdTurma;
                }

                var dadosRange = planilha.Cells[2, 1, planilha.Dimension.End.Row, headers.Length];
                dadosRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                planilha.Column(5).Style.Numberformat.Format = "hh:mm";
                planilha.Column(7).Style.Numberformat.Format = "R$ #,##0.00";
                planilha.Column(8).Style.Numberformat.Format = "dd/mm/yyyy";
                planilha.Column(9).Style.Numberformat.Format = "R$ #,##0.00";
                planilha.Column(10).Style.Numberformat.Format = "dd/mm/yyyy";
            }

            planilha.Cells[planilha.Dimension.Address].AutoFitColumns();

            return package.GetAsByteArray();
        }
    }
}