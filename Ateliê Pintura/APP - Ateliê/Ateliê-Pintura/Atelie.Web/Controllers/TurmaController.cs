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
    public class TurmaController : Controller
    {
        public ITurmaService _turmaService { get; set; }
        public ICursoService _cursoService { get; set; }
        public IAlunoService _alunoService { get; set; }

        public TurmaController(ITurmaService turmaService, ICursoService cursoService, IAlunoService alunoService)
        {
            _turmaService = turmaService;
            _cursoService = cursoService;
            _alunoService = alunoService;
        }

        public ActionResult Index()
        {
            ViewBag.CursosOptions = new SelectList(
                _cursoService.ListarTodos().Select(c => new { c.Id, c.Nome }),
                "Id", "Nome"
            );

            ViewBag.AlunosOptions = new SelectList(
                _alunoService.ListarTodos().Where(a => a.Ativo).Select(a => new { a.Id, a.Nome }),
                "Id", "Nome"
            );

            ViewBag.DiasDaSemanaOptions = new SelectList(
                Enum.GetValues(typeof(Atelie.Core.Enums.DiaDaSemana))
                    .Cast<Atelie.Core.Enums.DiaDaSemana>()
                    .Select(e => new { Value = (int)e, Text = e.GetDisplayName() }),
                "Value",
                "Text"
            );

            return View();
        }

        public JsonResult Listar(TurmaFiltroDTO filtro)
        {
            var lista = _turmaService.ListarTodosComFiltro(filtro);
            var data = new JsonFormat();

            var viewModel = TurmaMapper.ToViewModel(lista);

            data.Success = true;
            data.Object = viewModel;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Editar(long id)
        {
            var data = new JsonFormat();

            var turma = _turmaService.ObterPorId(id);

            var viewModel = TurmaMapper.ToViewModel(turma);

            data.Success = true;
            data.Object = viewModel;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarTurma(Turma turma)
        {
            var data = new JsonFormat();

            try
            {
                _turmaService.Salvar(turma);

                data.Success = true;
                data.Object = turma;

                data.MessageList.Add(string.Format(GlobalMessages.RegistradoComSucesso, GlobalMessages.Turma, turma.GetNomeTurma()));
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
        public JsonResult ExcluirTurma(long id)
        {
            var data = new JsonFormat();
            var obj = _turmaService.ObterPorId(id);

            try
            {
                _turmaService.Excluir(id);

                data.MessageList.Add(string.Format(GlobalMessages.ExcluidoComSucesso, GlobalMessages.Turma, obj.GetNomeTurma()));
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

        [HttpPost]
        public JsonResult SwitchTurmas(long id)
        {
            var data = new JsonFormat();

            var obj = _turmaService.ObterPorId(id);

            try
            {
                _turmaService.SwitchAtivo(id);

                data.MessageList.Add(string.Format(GlobalMessages.ExcluidoComSucesso, GlobalMessages.Turma, obj.GetNomeTurma()));
                data.Success = true;
            }
            catch (ValidationRuleException val)
            {
                data.Success = false;

                data.MessageList.AddRange(val.ValidationErrors.Values.SelectMany(list => list).ToList());
                data.FieldList.AddRange(val.ValidationErrors.Keys);
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
    }
}