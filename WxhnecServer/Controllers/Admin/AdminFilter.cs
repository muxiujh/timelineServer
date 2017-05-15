using System.Web;
using System.Web.Mvc;
using JCore;

namespace WxhnecServer
{
    public class AdminFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext) {
            if(HttpContext.Current.Session[G.super] == null) {
                filterContext.Result = new RedirectResult(AdminBaseController.c_pageLogin);
            }            
        }
    }
}