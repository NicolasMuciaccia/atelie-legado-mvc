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
    public class AlunoController : Controller
    {
        public IAlunoService _alunoService { get; set; }

        public AlunoController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        public ActionResult Index()
        {
            ViewBag.TiposDeContato = new SelectList(
                Enum.GetValues(typeof(Atelie.Core.Enums.TipoContato))
                    .Cast<Atelie.Core.Enums.TipoContato>()
                    .Select(e => new { Value = (int)e, Text = e.GetDisplayName() }),
                "Value",
                "Text"
            );

            return View();
        }

        public JsonResult Listar(AlunoFiltroDTO filtro)
        {
            var lista = _alunoService.ListarTodosComFiltro(filtro);
            var data = new JsonFormat();

            var viewModel = AlunoMapper.ToViewModel(lista, filtro.MesDasPresencasFiltro);

            data.Success = true;
            data.Object = viewModel;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Editar(long id)
        {
            var data = new JsonFormat();

            var aluno = _alunoService.ObterPorId(id);

            var viewModel = AlunoMapper.ToViewModel(aluno);

            data.Success = true;
            data.Object = viewModel;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarAluno(Aluno aluno)
        {
            var data = new JsonFormat();

            try
            {
                _alunoService.Salvar(aluno);

                data.Success = true;
                data.Object = aluno;

                data.MessageList.Add(string.Format(GlobalMessages.RegistradoComSucesso, GlobalMessages.Aluno, aluno.Nome));
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
        public JsonResult ExcluirAluno(long id)
        {
            var data = new JsonFormat();

            var obj = _alunoService.ObterPorId(id);

            try
            {
                _alunoService.Excluir(id);

                data.MessageList.Add(string.Format(GlobalMessages.ExcluidoComSucesso, GlobalMessages.Aluno, obj.Nome));
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
        public JsonResult SwitchAlunos(long id)
        {
            var data = new JsonFormat();

            var obj = _alunoService.ObterPorId(id);

            try
            {
                _alunoService.SwitchAtivo(id);

                data.MessageList.Add(string.Format(GlobalMessages.ExcluidoComSucesso, GlobalMessages.Aluno, obj.Nome));
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
    }
}