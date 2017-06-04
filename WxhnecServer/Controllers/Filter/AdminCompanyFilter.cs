using System.Web;
using System.Web.Mvc;
using JCore;
using System.Configuration;

namespace WxhnecServer
{
    public class AdminCompanyFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext) {
            while (true) {
                if (HttpContext.Current.Session[G.companyid] != null) {
                    break;
                }

                var companyidConfig = ConfigurationManager.AppSettings[G.companyid];
                if (!string.IsNullOrEmpty(companyidConfig)) {
                    HttpContext.Current.Session[G.companyid] = THelper.StringToInt(companyidConfig);
                    break;
                }

                filterContext.Result = new RedirectResult(AdminBaseController.c_pageLogin);
                break;
            }
        }
    }
}