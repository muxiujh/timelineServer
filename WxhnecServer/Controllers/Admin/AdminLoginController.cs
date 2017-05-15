using JCore;
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

        bool tryLogin(string password) {
            var result = false;
            while (true) {
                if (string.IsNullOrEmpty(password)) {
                    break;
                }

                if (ConfigurationManager.AppSettings["uname"] == password) {
                    Session[G.super] = 1;
                    result = true;
                    break;
                }

                var model = new CompanyModel();
                var row = model.GetCompany(password);
                if (row != null) {
                    Session[G.companyid] = row.id.Value;
                    Session[G.super] = 2;
                    result = true;
                }

                break;
            }
            return result;
        }

        public string LoginSave(FormCollection collection) {
            JObject jo = new JObject();

            if (tryLogin(collection["uname"])) {
                jo["msg"] = "login ok";
                jo["url"] = "/Admin/AdminIndex";
            }
            else {
                jo["msg"] = "try again";
            }

            return JsonConvert.SerializeObject(jo);
        }

        public RedirectResult LogOut() {
            Session.Clear();
            return Redirect("/AdminLogin/Login");
        }
    }
}