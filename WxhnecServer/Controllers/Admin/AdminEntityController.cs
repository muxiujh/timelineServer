using JCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WxhnecServer
{
    public abstract class AdminEntityController : AdminAuthController
    {
        protected const string c_t = "t";
        protected const string c_id = "id";
        protected const string c_success = "success";
        protected const string c_row = "row";
        protected const string c_pre = "pre_";
        protected const string c_template = "template";

        protected TQueryLogic m_logic = new TQueryLogic();
        protected string m_menuLeft;
        protected string m_pageList;
        protected string m_error;
        protected string m_table;
        protected bool m_isSave = false;

        [HttpGet]
        public string ValidateField(string name, string value, string t = null) {
            if (!initQuery(t, true)) {
                return toJson(m_error);
            }

            if(m_logic.ValidateField(name, value)) {
                m_jo[c_success] = true;
            }
            else {
                return JsonConvert.SerializeObject(m_logic.Errors);
            }

            return toJson();
        }

        [HttpGet]
        public dynamic Add(string t = null) {
            bool result = false;
            while (true) {
                if(!initQuery(t, true)) {
                    break;
                }

                if (!checkAdd()) {
                    break;
                }

                result = true;
                break;
            }

            if (m_isSave) {
                return result;
            }

            return rowView(result, nameof(Add));
        }

        [HttpPost]
        public string AddSave(FormCollection collection) {
            bool result = false;
            while (true) {
                m_isSave = true;
                if (!Add(Request.QueryString[c_t])) {
                    break;
                }

                // save
                m_logic.PresetDict = getPreset();
                if (!m_logic.SaveRow(collection)) {
                    m_error = m_logic.Error;
                    break;
                }

                finishAdd();
                result = true;
                break;
            }

            string url = string.Format("{0}?{1}={2}", m_pageList, c_t, m_table);
            return rowJson(result, url);
        }

        [HttpGet]
        public dynamic Modify(int id = 0, string t = null) {
            bool result = false;
            while (true) {
                if (!initQuery(t, true)) {
                    break;
                }

                if (!checkModify(id)) {
                    break;
                }

                result = true;
                break;
            }

            if (m_isSave) {
                return result;
            }

            return rowView(result, nameof(Modify));
        }

        [HttpPost]
        public string ModifySave(FormCollection collection) {
            bool result = false;
            while (true) {
                m_isSave = true;
                var id = THelper.StringToInt(collection.Get(c_id));
                if (!Modify(id, Request.QueryString[c_t])) {
                    break;
                }

                // save
                if (!m_logic.SaveRow(collection)) {
                    m_error = m_logic.Error;
                    break;
                }

                result = true;
                break;
            }

            return rowJson(result);
        }

        [HttpGet]
        public ActionResult List(string t = null, string c = null, int p = 1) {
            if (!initQuery(t)) {
                return error(m_error);
            }
            
            // listUI
            TListUI listUI = new TListUI();
            ViewBag.listDict = listUI.Class2UI(m_logic.FullName);
            ViewBag.t = t;

            // page
            int pageSize = THelper.StringToInt(m_adminConfig["pageSize"]);
            m_logic.PresetDict = getPreset();
            var result = m_logic.Condition(c).GetList(p, pageSize, true);
            ViewBag.spage = m_logic.Paging;
            ViewBag.title = m_logic.GetTitle();

            // filter
            var searchFields = listUI.Class2Search(m_logic.FullName);
            ViewBag.filter = THelper.GetFilter(searchFields, m_logic.CompareDict);
            if (c != null) {
                ViewBag.filterString = c.Replace(G.Split1, "&").Replace(G.Split2, "=");
            }

            return View(result);
        }

        string rowJson(bool result, string url = null) {
            if (result) {
                m_jo[c_msg] = G.L["submit_ok"];
                m_jo[c_success] = true;
                m_jo[c_url] = url;
            }
            else {
                return toJsonRaw(m_logic.Errors);
            }

            return toJson();
        }

        ActionResult rowView(bool result, string mode) {
            if (!result) {
                return error(m_error);
            }

            if (m_logic.Row == null) {
                m_logic.CreateRow();
            }

            string modeSave = mode + "Save";
            ViewBag.title = m_logic.GetTitle();
            ViewBag.UrlSave = Request.RawUrl.Replace(mode, modeSave);
            ViewBag.UrlValidate = Request.RawUrl.Replace(mode, nameof(ValidateField));
            return View(c_row, m_logic.Row);
        }

        bool initQuery(string table, bool isRow = false) {
            bool result = false;
            while (true) {
                m_table = table;
                if (!checkTable()) {
                    break;
                }

                if (!m_logic.InitQuery(GetType().Namespace, m_table, isRow)) {
                    m_error = m_logic.Error;
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

        protected virtual bool checkTable() {
            bool result = false;
            while (true) {
                if (m_menuLeft == null) {
                    result = true;
                    break;
                }

                string configFile = string.Format("{0}{1}/menu/{2}/{3}.json", m_serverDir, m_adminConfig["resource"], Session[G.super], m_menuLeft);
                var menu = ConfigHelper.LoadConfig(configFile);
                if (menu == null) {
                    break;
                }

                var list = ConfigHelper.GetList(menu, c_template);
                if (list.Count == 0) {
                    break;
                }

                if (!list.Exists(t => t == m_table)) {
                    m_error = G.L["no_permission"];
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

        protected virtual bool checkAdd() {
            return true;
        }

        protected virtual bool finishAdd() {
            return true;
        }

        protected virtual bool checkModify(int id) {
            return m_logic.CheckRow(id, !m_isSave);
        }

        protected virtual Dictionary<string, object> getPreset() {
            return null;
        }

    }
}