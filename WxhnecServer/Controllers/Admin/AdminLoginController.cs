using JCore;
using System.Configuration;
using System.IO;
using System.Web.Mvc;

namespace WxhnecServer
{
    public class AdminLoginController : AdminBaseController
    {
        const string c_code = "code";
        const string c_uname = "uname";
        const string c_try = "try";

        public ActionResult Login() {
            ViewBag.bodyClass = "admin_login";
            return View();
        }

        //  1: ok
        //  0: code error
        // -1: password error
        // -2: too many time
        //
        int tryLogin(string password, string code) {
            var result = 0;
            while (true) {
                if (string.IsNullOrEmpty(password)) {
                    break;
                }

                if (Session[c_code] == null) {
                    break;
                }

                if (string.IsNullOrEmpty(code)) {
                    if (ConfigurationManager.AppSettings[c_uname] == password) {
                        Session[G.super] = TS.s1;
                        result = 1;
                    }
                    break;
                }

                if(code != Session[c_code].ToString()) {
                    break;
                }

                int tryLimit = THelper.StringToInt(ConfigurationManager.AppSettings[c_try]);
                if (Session[c_try] != null && (int)Session[c_try] >= tryLimit) {
                    result = -2;
                    break;
                }

                var model = new CompanyModel();
                var row = model.GetCompany(password);
                if (row != null) {
                    Session[G.companyid] = row.id.Value;
                    Session[G.super] = TS.s2;
                    Session.Remove(c_try);
                    result = 1;
                }
                else {
                    result = -1;
                    if(Session[c_try] == null) {
                        Session[c_try] = 1;
                    }
                    else {
                        Session[c_try] = (int)Session[c_try] + 1;
                    }
                }

                Session.Remove(c_code);
                break;
            }

            return result;
        }

        public string LoginSave(FormCollection collection) {
            var result = tryLogin(collection[c_uname], collection[c_code]);
            switch (result) {
                case 1:
                    m_jo[c_msg] = G.L["login_ok"];
                    m_jo[c_url] = c_pageMain;
                    break;
                case 0:
                    m_jo[c_msg] = G.L["code_error"];
                    break;
                case -1:
                    m_jo[c_msg] = G.L["password_error"];
                    m_jo[c_url] = "reload";
                    break;
                case -2:
                    m_jo[c_msg] = G.L["try_error"];
                    break;
            }
            return toJson();
        }

        public RedirectResult LogOut() {
            Session.Clear();
            return Redirect(c_pageLogin);
        }

        public ActionResult Image() {
            var random = new JRandom();
            Session[c_code] = random.Next();

            MemoryStream stream = new MemoryStream();
            PsHelper.Text2Image(stream, random.Content);
            return File(stream.ToArray(), "image/jpeg");
        }
    }
}