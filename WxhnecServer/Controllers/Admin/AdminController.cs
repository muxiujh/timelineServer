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
        const string c_menuDir = "menuDir.json";
        const string c_menuTop = "menuTop.json";

        protected ActionResult m_login = null;
        protected dynamic m_adminConfig;

        public AdminController() {
            ViewBag.controller = this;
            ViewBag.ui = G.Config["UI"]["ui"];

            m_adminConfig = G.Config["AdminUI"];
            ViewBag.resourceAdmin = m_adminConfig["resource"];
        }

        protected bool checkLogin() {
            bool result = true;
            while (true) {
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["debug"])) {
                    break;
                }

                if (Session[G.super] == null) {
                    m_login = Redirect("/AdminLogin/Login");
                    result = false;
                }

                break;
            }
            return result;
        }

        public ActionResult Info() {
            return View();
        }

        public ActionResult AdminIndex() {
            if (!checkLogin())
                return m_login;
            
            return View();
        }

        string getMenuDir() {
            string result = null;
            while (true) {
                var super = Session[G.super];
                if (super == null) {
                    break;
                }

                string menuDir = Server.MapPath("~") + m_adminConfig["resource"] + "/";
                string menuDirFile = menuDir + c_menuDir;
                JObject menuDirConfig = ConfigHelper.LoadConfig(menuDirFile) as JObject;
                if(menuDirConfig == null) {
                    break;
                }

                var dir = menuDirConfig[super.ToString()];
                if(dir == null) {
                    break;
                }
                
                result = menuDir + dir.ToString() + "/";
                break;
            }
            return result;
        }

        public ActionResult AdminTop(int id = 0) {
            ViewBag.bodyClass = "admin_top";
            ViewBag.key = id;

            string menuConfig = getMenuDir() + c_menuTop;
            var menu = ConfigHelper.LoadConfigIncludeSubFirst(menuConfig, "left", "sub");            
            return View(menu);
        }

        public ActionResult AdminLeft(int id = 0, string left = null) {
            ViewBag.bodyClass = "admin_left";
            ViewBag.key = id;

            var menuLeft = ConfigHelper.LoadConfig(getMenuDir() + left);
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