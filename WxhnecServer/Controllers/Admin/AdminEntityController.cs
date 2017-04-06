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
        [HttpPost]
        public string RowSave(FormCollection collection) {
            string t = Request.QueryString["t"];
            if (!checkModel(t))
                return Error(true);

            var entity = getTEntity(t);
            TEntityUI entityUI = new TEntityUI();

            var row = entityUI.UI2Row(collection, entity.TType);
            var id = collection.Get("id");
            bool result;
            if (string.IsNullOrEmpty(id)) {
                result = entity.Add(row) != null;
            }
            else {
                result = entity.Modify(row);
            }

            JObject jo = new JObject();
            if (result) {
                jo["msg"] = "success!";

                if (string.IsNullOrEmpty(id)) {
                    jo["url"] = "/AdminEntity/List?t=" + t;
                }
            }
            else {
                Dictionary<string, string> errors = entity.Errors;
                var error = errors.First();
                jo["msg"] = error.Key + " " + error.Value; ;
            }

            return JsonConvert.SerializeObject(jo);
        }

        [HttpGet]
        public ActionResult Row(int id = 0, string t = null) {
            if (!checkLogin())
                return m_login;

            if (!checkModel(t))
                return Error();

            // entityClass
            var entity = getTEntity(t);
            ViewBag.title = entity.GetTitle();

            // row
            object row = null;
            if (id > 0) {
                string detail = t + "_detail";
                if (entity.TType.GetProperty(detail) == null) {
                    detail = null;
                }
                row = entity.FindRow(id, detail);
            }
            else {
                row = Activator.CreateInstance(entity.TType);
            }

            ViewBag.UrlSave = Request.RawUrl.Replace("Row", "RowSave");

            return View(row);
        }

        [HttpGet]
        public ActionResult List(string t = null, int p = 1) {
            if (!checkLogin())
                return m_login;

            if (!checkModel(t))
                return Error();

            // listClass
            var list = getTList(t);
            ViewBag.title = list.GetTitle();

            // listUI
            TListUI listUI = new TListUI();
            ViewBag.listDict = listUI.Class2UI(getFullName(t));
            ViewBag.t = t;

            // page
            int pageSize = THelper.StringToInt(m_adminConfig["pageSize"]);
            int total = list.Count();
            ViewBag.spage = new SPage(p, pageSize, total);

            // list
            var result = list.TLimit(pageSize, p).ToList();

            return View(result);
        }

        bool checkModel(string t) {
            bool result = false;
            while (true) {
                if (string.IsNullOrWhiteSpace(t)) {
                    m_error = "t is need.";
                    break;
                }

                Type type = Type.GetType(getFullName(t));
                if (type == null) {
                    m_error = "no " + t;
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

        string getFullName(string t) {
            return GetType().Namespace + "." + t;
        }

        dynamic getTEntity(string t) {
            Type generic = typeof(TEntityLogic<>);
            return THelper.CreateInstance(generic, getFullName(t));
        }

        dynamic getTList(string t) {
            Type generic = typeof(TListLogic<>);
            return THelper.CreateInstance(generic, getFullName(t));
        }
    }
}