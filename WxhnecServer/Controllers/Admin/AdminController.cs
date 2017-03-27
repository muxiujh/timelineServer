using JCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace WxhnecServer
{
    public class AdminController : Controller
    {
        protected ActionResult m_login = null;
        protected dynamic m_adminConfig;
        protected string m_error;

        public AdminController() {
            ViewBag.controller = this;
            ViewBag.ui = G.Config["UI"]["ui"];
            ViewBag.resource = G.Config["UI"]["resource"];

            m_adminConfig = G.Config["AdminUI"];
            ViewBag.resourceAdmin = m_adminConfig["resource"];
        }

        protected bool checkLogin() {
            bool result = true;
            while (true) {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["debug"])) {
                    break;
                }

                if (Session["adminLogin"] == null) {
                    m_login = Redirect("/AdminLogin/Login");
                    result = false;
                }

                break;
            }
            return result;
        }

        public dynamic Error(bool isJson = false) {
            if (isJson) {
                JObject jo = new JObject();
                jo["msg"] = m_error;
                return JsonConvert.SerializeObject(jo);
            }
            else {
                ViewBag.Error = m_error;
                return View("~/Views/Admin/Error.cshtml");
            }
        }

        public ActionResult Info() {
            return View();
        }

        public ActionResult AdminIndex() {
            if (!checkLogin())
                return m_login;
            
            return View();
        }

        public ActionResult AdminTop(int id = 0) {
            ViewBag.bodyClass = "admin_top";
            ViewBag.key = id;

            string menuConfig = m_adminConfig["menu"];
            var menu = ConfigHelper.LoadConfigIncludeSubFirst(menuConfig, "left", "sub");            
            return View(menu);
        }

        public ActionResult AdminLeft(int id = 0, string left = null) {
            ViewBag.bodyClass = "admin_left";
            ViewBag.key = id;

            string menuConfig = m_adminConfig["menu"];
            string menuDir = Path.GetDirectoryName(menuConfig) + "/";
            var menuLeft = ConfigHelper.LoadConfig(menuDir + left);
            return View(menuLeft);
        }
        
        public string Refresh() {
            CacheExtension.RemoveAll();
            JObject jo = new JObject();
            jo["msg"] = "ok!";

            return JsonConvert.SerializeObject(jo);
        }

    }
}