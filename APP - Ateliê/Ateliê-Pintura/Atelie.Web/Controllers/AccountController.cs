using System.Web;
using System;
using System.Web.Mvc;
using Atelie.Web.Security;
using Atelie.Web.ViewModels;
using Atelie.Core.JsonModel;
using Atelie.Core.Resources;

namespace Atelie.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            var data = new JsonFormat();

            if (model.Username == "osana" && model.Password == "1971.")
            {
                var token = TokenManager.GenerateToken(model.Username);

                Response.SetCookie(new HttpCookie("jwt_token", token)
                {
                    HttpOnly = true,
                    Expires = DateTime.UtcNow.AddHours(2),
                    Path = "/"
                });

                data.Success = true;

                return Json(data, JsonRequestBehavior.AllowGet);
            }

            data.Success = false;
            data.MessageList.Add(GlobalMessages.UsuarioSenhaInvalido);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (Request.Cookies["jwt_token"] != null)
            {
                var cookie = new HttpCookie("jwt_token")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Path = "/"
                };
                Response.Cookies.Add(cookie);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}