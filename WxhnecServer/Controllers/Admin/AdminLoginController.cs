using JCore;
using System.Configuration;
using System.Web.Mvc;

namespace WxhnecServer
{
    public class AdminLoginController : AdminBaseController
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
            if (tryLogin(collection["uname"])) {
                m_jo[c_msg] = "login ok";
                m_jo[c_url] = c_pageMain;
            }
            else {
                m_jo[c_msg] = "try again";
            }

            return toJson();
        }

        public RedirectResult LogOut() {
            Session.Clear();
            return Redirect(c_pageLogin);
        }
    }
}