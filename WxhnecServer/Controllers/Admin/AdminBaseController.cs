using JCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;

namespace WxhnecServer
{
    abstract public class AdminBaseController : Controller
    {
        public const string c_pageLogin = "/AdminLogin/Login";
        public const string c_pageMain = "/Admin/Index";
        public const string c_htmlError = "~/Views/Admin/Error.cshtml";
        public const string c_msg = "msg";
        public const string c_url = "url";

        protected JObject m_jo = new JObject();
        protected dynamic m_adminConfig;

        public AdminBaseController() {
            ViewBag.controller = this;
            ViewBag.ui = G.Config["UI"]["ui"];

            m_adminConfig = G.Config["AdminUI"];
            ViewBag.resourceAdmin = m_adminConfig["resource"];
        }

        protected string toJson(string msg = null) {
            if (msg != null) {
                m_jo[c_msg] = msg;
            }
            return JsonConvert.SerializeObject(m_jo);
        }

        protected ActionResult error(string msg = null) {
            if(msg != null) {
                ViewBag.Error = msg;
            }
            else if(m_jo[c_msg] != null) {
                ViewBag.Error = m_jo[c_msg].ToString();
            }
            return View(c_htmlError);
        }
    }
}