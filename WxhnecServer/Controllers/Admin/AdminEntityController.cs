using JCore;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WxhnecServer
{
    public class AdminEntityController : AdminAuthController
    {
        const string c_pageList = "/AdminEntity/List";
        const string c_t = "t";
        const string c_id = "id";

        TQueryLogic m_logic = new TQueryLogic();
        string m_namespace { get { return GetType().Namespace; } }

        [HttpPost]
        public string RowSave(FormCollection collection) {
            string t = Request.QueryString[c_t];

            if (!m_logic.InitQuery(m_namespace, t, true)) {
                return toJson(m_logic.Error);
            }

            m_logic.PresetDict = getPreset(t, G.Preset);
            bool result = m_logic.SaveRow(collection);

            if (result) {
                m_jo[c_msg] = "success!";

                var id = collection.Get(c_id);
                if (string.IsNullOrEmpty(id)) {
                    m_jo[c_url] = c_pageList + "?" + c_t + "=" + t;
                }
            }
            else {
                m_jo[c_msg] = m_logic.Error;
            }

            return toJson();
        }

        [HttpGet]
        public ActionResult Row(int id = 0, string t = null) {
            if (!m_logic.InitQuery(m_namespace, t, true)) {
                return error(m_logic.Error);
            }

            if (id == 0) {
                var presetDict = getPreset(t, G.PresetId);
                var first = presetDict.FirstOrDefault();
                id = THelper.StringToInt(first.Value);
            }

            object row = m_logic.GetRow(id);

            ViewBag.title = m_logic.GetTitle();
            ViewBag.UrlSave = Request.RawUrl.Replace("Row", "RowSave");

            return View(row);
        }

        [HttpGet]
        public ActionResult List(string t = null, string c = null, int p = 1) {
            if (!m_logic.InitQuery(m_namespace, t)) {
                return error(m_logic.Error);
            }
            
            // listUI
            TListUI listUI = new TListUI();
            ViewBag.listDict = listUI.Class2UI(m_logic.FullName);
            ViewBag.t = t;

            // page
            int pageSize = THelper.StringToInt(m_adminConfig["pageSize"]);
            m_logic.PresetDict = getPreset(t, G.Preset);
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

        Dictionary<string, object> getPreset(string table, Dictionary<string, List<string>> preset) {
            var session = this.FilterSession(THelper.GetKeys(preset));
            return THelper.GetPreset(table, session, preset);             
        }

    }
}