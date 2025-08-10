using System;
using System.Web.Mvc;
using Atelie.Core.JsonModel;
using Atelie.Infrastructure.Abstractions.Repositories;
using Atelie.Infrastructure.Abstractions.Services;
using Atelie.Infrastructure.Services.DTOs;
using Atelie.Web.Security;

namespace Atelie.Web.Controllers
{
    [JwtAuthorize]
    public class HomeController : Controller
    {
        private readonly IRelatorioExcelService _relatorioExcelService;
        private readonly IViewRelatorioPagamentoService _viewRelatorioPagamentoService;

        public HomeController(IRelatorioExcelService relatorioExcelService, IViewRelatorioPagamentoService viewRelatorioPagamentoService)
        {
            _relatorioExcelService = relatorioExcelService;
            _viewRelatorioPagamentoService = viewRelatorioPagamentoService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExportarRelatorio(ViewRelatorioPagamentoFiltroDTO filtro)
        {
            try
            {
                byte[] fileBytes = _relatorioExcelService.GerarPlanilhaExcel(filtro);
                string fileName = _relatorioExcelService.FormatarNomeExcel(filtro);
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(fileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocorreu um erro ao gerar o relatório.";
                return RedirectToAction("Index");
            }
        }

        public JsonResult ObterDadosGrafico(ViewRelatorioPagamentoFiltroDTO filtro)
        {
            try
            {
                var data = new JsonFormat();

                var dadosGrafico = _viewRelatorioPagamentoService.ObterDadosGrafico(filtro);

                data.Success = true;
                data.Object = dadosGrafico;

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = "Erro ao obter dados do gráfico." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}