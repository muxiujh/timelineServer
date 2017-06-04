using System.Web;
using System.Web.Mvc;
using JCore;

namespace WxhnecServer
{
    public class AdminSuperFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext) {
            while (true) {
                var super = HttpContext.Current.Session[G.super];
                if (super != null && AdminEnum.super.Equals(super)) {
                    break;
                }

                filterContext.Result = new RedirectResult(AdminBaseController.c_pageLogin);
                break;
            }
        }
    }
}