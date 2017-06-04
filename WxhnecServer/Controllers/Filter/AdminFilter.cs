using System.Web;
using System.Web.Mvc;
using JCore;
using System.Configuration;

namespace WxhnecServer
{
    public class AdminFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext) {
            while (true) {
                if (HttpContext.Current.Session[G.super] != null) {
                    break;
                }

                var superConfig = ConfigurationManager.AppSettings[G.super];
                if (!string.IsNullOrEmpty(superConfig)) {
                    HttpContext.Current.Session[G.super] = AdminEnum.super;
                    break;
                }

                filterContext.Result = new RedirectResult(AdminBaseController.c_pageLogin);
                break;
            }
        }
    }
}