using Atelie.Infrastructure.Services.DTOs;

public interface IRelatorioExcelService
{
    byte[] GerarPlanilhaExcel(ViewRelatorioPagamentoFiltroDTO filtro);
    string FormatarNomeExcel(ViewRelatorioPagamentoFiltroDTO filtro);
}