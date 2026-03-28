using System;
using System.Linq;
using System.Web.Mvc;
using Atelie.Core.Exceptions;
using Atelie.Core.JsonModel;
using Atelie.Core.Resources;
using Atelie.Core.Utils;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Services;
using Atelie.Infrastructure.Services.DTOs;
using Atelie.Web.Mappers;
using Atelie.Web.Security;

namespace Atelie.Web.Controllers
{
    [JwtAuthorize]
    public class PagamentoController : Controller
    {
        public IPagamentoService _pagamentoService { get; set; }
        public ITurmaService _turmaService { get; set; }
        public IAlunoService _alunoService { get; set; }

        public PagamentoController(IPagamentoService pagamentoService, ITurmaService turmaService, IAlunoService alunoService)
        {
            _pagamentoService = pagamentoService;
            _turmaService = turmaService;
            _alunoService = alunoService;
        }

        public ActionResult Index(long? id)
        {
            ViewBag.TipoPagamento = new SelectList(
                Enum.GetValues(typeof(Atelie.Core.Enums.TipoPagamento))
                    .Cast<Atelie.Core.Enums.TipoPagamento>()
                    .Select(e => new { Value = (int)e, Text = e.GetDisplayName() }),
                "Value",
                "Text"
            );

            ViewBag.ValorPago = new SelectList(
                ValorPagamentoSelectUtil.Lista,
                "Key",
                "Value"
            );

            ViewBag.AlunosOptions = new SelectList(
                _alunoService.ListarTodos().Select(a => new { a.Id, a.Nome }),
                "Id", "Nome"
            );

            ViewBag.AlunoIdFiltroInicial = id;

            return View();
        }

        public JsonResult Listar(PagamentoFiltroDTO filtro)
        {
            var lista = _pagamentoService.ListarTodosComFiltro(filtro);
            var data = new JsonFormat();

            var viewModel = PagamentoMapper.ToViewModel(lista);

            data.Success = true;
            data.Object = viewModel;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Editar(long id)
        {
            var data = new JsonFormat();

            var pagamento = _pagamentoService.ObterPorId(id);

            var viewModel = PagamentoMapper.ToViewModel(pagamento);

            data.Success = true;
            data.Object = viewModel;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarPagamento(Pagamento pagamento)
        {
            var data = new JsonFormat();

            try
            {
                _pagamentoService.Salvar(pagamento);

                data.Success = true;
                data.Object = pagamento;

                data.MessageList.Add(string.Format(GlobalMessages.RegistradoComSucesso, GlobalMessages.Pagamento, pagamento.GetPagamentoNome()));
            }
            catch (ValidationRuleException val)
            {
                data.Success = false;

                data.MessageList.AddRange(val.ValidationErrors.Values.SelectMany(list => list).ToList());
                data.FieldList.AddRange(val.ValidationErrors.Keys);
            }
            catch (Exception ex)
            {
                data.Success = false;
                data.MessageList.Add(string.Format(GlobalMessages.ErroInesperado, ex.Message));
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ExcluirPagamento(long id)
        {
            var data = new JsonFormat();

            var obj = _pagamentoService.ObterPorId(id);

            try
            {
                _pagamentoService.Excluir(id);

                data.MessageList.Add(string.Format(GlobalMessages.ExcluidoComSucesso, GlobalMessages.Pagamento, obj.GetPagamentoNome()));
                data.Success = true;
            }
            catch (EntityNotFoundException ent)
            {
                data.Success = false;
                data.MessageList.Add(ent.Message);
            }
            catch (Exception ex)
            {
                data.Success = false;
                data.MessageList.Add(string.Format(GlobalMessages.ErroInesperado, ex.Message));
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObterTurmasPorAlunoId(long alunoId)
        {
            var turmas = _turmaService.ObterTurmaPorAlunoId(alunoId);
            return Json(turmas, JsonRequestBehavior.AllowGet);
        }
    }
}