using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Web.Mvc;

namespace WxhnecServer
{
    public class AdminLoginController : AdminController
    {
        public ActionResult Login() {
            ViewBag.bodyClass = "admin_login";
            return View();
        }

        public string LoginSave(FormCollection collection) {
            JObject jo = new JObject();
            if (ConfigurationManager.AppSettings["uname"] == collection["uname"]) {
                Session["adminLogin"] = true;
                jo["msg"] = "login ok";
                jo["url"] = "/Admin/AdminIndex";
            }
            else {
                jo["msg"] = "try again";
            }

            return JsonConvert.SerializeObject(jo);
        }
    }
}