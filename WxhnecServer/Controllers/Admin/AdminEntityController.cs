using JCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WxhnecServer
{
    public class AdminEntityController : AdminController
    {
        TQueryLogic m_logic = new TQueryLogic();
        string m_namespace { get { return GetType().Namespace; } }

        [HttpPost]
        public string RowSave(FormCollection collection) {
            string t = Request.QueryString["t"];

            if (!m_logic.InitQuery(m_namespace, t)) {
                return error();
            }

            bool result = m_logic.SaveRow(collection);

            JObject jo = new JObject();
            if (result) {
                jo["msg"] = "success!";

                var id = collection.Get("id");
                if (string.IsNullOrEmpty(id)) {
                    jo["url"] = "/AdminEntity/List?t=" + t;
                }
            }
            else {
                jo["msg"] = m_logic.Error;
            }

            return JsonConvert.SerializeObject(jo);
        }

        [HttpGet]
        public ActionResult Row(int id = 0, string t = null) {
            if (!checkLogin())
                return m_login;

            if (!m_logic.InitQuery(m_namespace, t)) {
                return error();
            }

            object row = m_logic.GetRow(id);

            ViewBag.title = m_logic.GetTitle();
            ViewBag.UrlSave = Request.RawUrl.Replace("Row", "RowSave");

            return View(row);
        }

        [HttpGet]
        public ActionResult List(string t = null, int p = 1) {
            if (!checkLogin())
                return m_login;

            if (!m_logic.InitQuery(m_namespace, t)) {
                return error();
            }

            // listUI
            TListUI listUI = new TListUI();
            ViewBag.listDict = listUI.Class2UI(m_logic.FullName);
            ViewBag.t = t;

            // page
            int pageSize = THelper.StringToInt(m_adminConfig["pageSize"]);
            var result = m_logic.GetList(p, pageSize, true);
            ViewBag.spage = m_logic.Paging;
            ViewBag.title = m_logic.GetTitle();

            return View(result);
        }

        public dynamic error(bool isJson = false) {
            if (isJson) {
                JObject jo = new JObject();
                jo["msg"] = m_logic.Error;
                return JsonConvert.SerializeObject(jo);
            }
            else {
                ViewBag.Error = m_logic.Error;
                return View("~/Views/Admin/Error.cshtml");
            }
        }

    }
}