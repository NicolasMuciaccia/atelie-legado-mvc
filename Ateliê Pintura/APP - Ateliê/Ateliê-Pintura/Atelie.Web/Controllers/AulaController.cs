using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Atelie.Core.Exceptions;
using Atelie.Core.JsonModel;
using Atelie.Core.Resources;
using Atelie.Domain.Entities;
using Atelie.Infrastructure.Abstractions.Services;
using Atelie.Infrastructure.Services.DTOs;
using Atelie.Web.Mappers;
using Atelie.Web.Security;

namespace Atelie.Web.Controllers
{
    [JwtAuthorize]
    public class AulaController : Controller
    {
        public IAulaService _aulaService { get; set; }
        public IPresencaService _presencaService { get; set; }
        public ITurmaService _turmaService { get; set; }
        public IAlunoService _alunoService { get; set; }

        public AulaController(IAulaService aulaService, IPresencaService presencaService, ITurmaService turmaService, IAlunoService alunoService)
        {
            _aulaService = aulaService;
            _presencaService = presencaService;
            _turmaService = turmaService;
            _alunoService = alunoService;
        }

        public ActionResult Index(long? id)
        {
            var turmasComAlunos = _turmaService.ListarTodos();

            ViewBag.TurmasDictionary = turmasComAlunos.ToDictionary(
                turma => turma.Id.ToString(),
                turma => turma.Alunos
                               .Where(a => a.Ativo)
                               .Select(aluno => new { aluno.Id, aluno.Nome })
            );

            ViewBag.TurmasSelectList = new SelectList(
                _turmaService.ListarTodos().Where(t => t.Ativo).Select(t => new { Id = t.Id, Nome = t.GetNomeTurma() }),
                "Id", "Nome"
            );

            ViewBag.AlunosSelectList = new SelectList(
                _alunoService.ListarTodos().Where(a => a.Ativo).Select(a => new { a.Id, a.Nome }),
                "Id", "Nome"
            );

            ViewBag.TurmaIdFiltroInicial = id;

            return View();
        }

        public JsonResult Listar(AulaFiltroDTO filtro)
        {
            var lista = _aulaService.ListarTodosComFiltro(filtro);
            var data = new JsonFormat();

            var viewModel = AulaMapper.ToViewModel(lista);

            data.Success = true;
            data.Object = viewModel;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Editar(long id)
        {
            var data = new JsonFormat();

            var aula = _aulaService.ObterPorId(id);

            var viewModel = AulaMapper.ToViewModel(aula);

            data.Success = true;
            data.Object = viewModel;

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarAula(Aula aula, List<Presenca> presencas)
        {
            var data = new JsonFormat();

            try
            {
                aula = _aulaService.Salvar(aula);

                if(presencas != null)
                    foreach (var presenca in presencas)
                        presenca.Aula = aula;

                _presencaService.SincronizarPresencas(presencas);

                data.Success = true;
                data.Object = aula;

                data.MessageList.Add(string.Format(GlobalMessages.RegistradoComSucesso, GlobalMessages.Aula, aula.GetAulaNome()));
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
        public JsonResult ExcluirAula(long id)
        {
            var data = new JsonFormat();

            var obj = _aulaService.ObterPorId(id);

            try
            {
                _aulaService.Excluir(id);

                data.MessageList.Add(string.Format(GlobalMessages.ExcluidoComSucesso, GlobalMessages.Aula, obj.GetAulaNome()));
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