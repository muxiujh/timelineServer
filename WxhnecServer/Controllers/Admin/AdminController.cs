using JCore;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;

namespace WxhnecServer
{
    public class AdminController : AdminAuthController
    {
        const string c_menuDir = "menuDir.json";
        const string c_menuTop = "menuTop.json";

        public ActionResult Info() {
            return View();
        }

        public ActionResult Index() {
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
            return toJson(G.L["refresh_ok"]);
        }

    }
}