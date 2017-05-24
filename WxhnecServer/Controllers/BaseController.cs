using JCore;
using System.Web.Mvc;
using System.Web.Routing;

namespace WxhnecServer
{
    abstract public class BaseController : Controller
    {
        protected string m_serverDir;
        protected override void Initialize(RequestContext requestContext) {
            base.Initialize(requestContext);
            m_serverDir = Server.MapPath("~");

            if (G.L == null || !G.L.Success) {
                string langFile = m_serverDir + "Resources/lang/cn.json";
                G.L = new JLang(langFile);
            }
        }
    }
}