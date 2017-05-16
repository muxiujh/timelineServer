using JCore;
using System.Web.Mvc;
using System.Web.Routing;

namespace WxhnecServer
{
    abstract public class BaseController : Controller
    {
        protected override void Initialize(RequestContext requestContext) {
            base.Initialize(requestContext);
            if(G.L == null || !G.L.Success) {
                string langFile = Server.MapPath("~") + "Resources/lang/cn.json";
                G.L = new JLang(langFile);
            }
        }
    }
}