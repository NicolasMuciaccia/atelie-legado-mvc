using System;
using System.Web.Mvc;
using Atelie.Core.Exceptions;
using Atelie.Core.JsonModel;
using Atelie.Core.Resources;
using Atelie.Infrastructure.Abstractions.Services;
using Atelie.Web.Security;

namespace Atelie.Web.Controllers
{
    [JwtAuthorize]
    public class PresencaController : Controller
    {
        public IPresencaService _presencaService { get; set; }

        public PresencaController(IPresencaService presencaService)
        {
            _presencaService = presencaService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ExcluirPresenca(long id)
        {
            var data = new JsonFormat();

            var obj = _presencaService.ObterPorId(id);

            try
            {
                _presencaService.Excluir(id);

                data.MessageList.Add(string.Format(GlobalMessages.ExcluidoComSucesso, GlobalMessages.Presenca, obj.GetPresencaNome()));
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