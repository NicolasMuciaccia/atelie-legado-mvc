using System;
using System.Linq;
using System.Web.Mvc;
using Atelie.Core.Exceptions;
using Atelie.Core.JsonModel;
using Atelie.Core.Resources;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Services;
using Atelie.Web.Mappers;
using Atelie.Web.Security;

namespace Atelie.Web.Controllers
{
    [JwtAuthorize]
    public class CursoController : Controller
    {
        public ICursoService _cursoService { get; set; }
        public ITurmaService _turmaService { get; set; }

        public CursoController(ICursoService cursoService, ITurmaService turmaService)
        {
            _cursoService = cursoService;
            _turmaService = turmaService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar()
        {
            var lista = _cursoService.ListarTodos();
            var data = new JsonFormat();

            var viewModel = CursoMapper.ToViewModel(lista);

            data.Success = true;
            data.Object = viewModel;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Editar(long id)
        {
            var data = new JsonFormat();

            var curso = _cursoService.ObterPorId(id);

            var viewModel = CursoMapper.ToViewModel(curso);

            data.Success = true;
            data.Object = viewModel;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarCurso(Curso curso)
        {
            var data = new JsonFormat();

            try
            {
                _cursoService.Salvar(curso);

                data.Success = true;
                data.Object = curso;

                data.MessageList.Add(string.Format(GlobalMessages.RegistradoComSucesso, GlobalMessages.Curso, curso.Nome));
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
        public JsonResult ExcluirCurso(long id)
        {
            var data = new JsonFormat();

            var obj = _cursoService.ObterPorId(id);

            try
            {
                _cursoService.Excluir(id);

                data.MessageList.Add(string.Format(GlobalMessages.ExcluidoComSucesso, GlobalMessages.Curso, obj.Nome));
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
        public JsonResult SwitchCursos(long id)
        {
            var data = new JsonFormat();

            var obj = _cursoService.ObterPorId(id);
            var turmaIds = _cursoService.ObterTurmaIdPorCursoId(id);

            try
            {
                _turmaService.DesativarEmLote(turmaIds);

                _cursoService.SwitchAtivo(id);

                data.MessageList.Add(string.Format(GlobalMessages.AlteradoComSucesso, GlobalMessages.Curso, obj.Nome));
                data.Success = true;
            }
            catch (EntityNotFoundException ent)
            {
                data.Success = false;
                data.MessageList.Add(ent.Message);
            }
            catch (BusinessRuleException bus)
            {
                data.Success = false;
                data.MessageList.AddRange(bus.BusinessErrors);
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