using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Atelie.Web.Security
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var cookie = filterContext.HttpContext.Request.Cookies["jwt_token"];
            var token = cookie?.Value;

            var principal = TokenManager.ValidateToken(token);

            if (string.IsNullOrEmpty(token) || principal == null)
            {
                HandleUnauthorized(filterContext);
                return;
            }

            HttpContext.Current.User = principal;
            base.OnAuthorization(filterContext);
        }

        private void HandleUnauthorized(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult( 
                new RouteValueDictionary (
                    new { controller = "Account", action = "Login" }
                )
            );
        }
    }
}