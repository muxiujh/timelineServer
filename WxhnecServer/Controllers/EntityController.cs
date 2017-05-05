using JCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WxhnecServer
{
    public class EntityController : Controller
    {
        TQueryLogic m_logic = new TQueryLogic();
        string m_namespace { get { return GetType().Namespace; } }

        [HttpGet]
        public string List(string t = null, int p = 1) {
            if (!m_logic.InitQuery(m_namespace, t)) {
                return error();
            }

            var result = m_logic.GetList(p, 100);

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        public string Row(int id = 0, string t = null) {
            if (!m_logic.InitQuery(m_namespace, t)) {
                return error();
            }

            object row = m_logic.GetRow(id);

            TEntityUI m_entityUI = new TEntityUI();
            var result = new Dictionary<string, object>();
            m_entityUI.Row2UI(row, dict => {
                if (dict != null) {
                    result.Add(dict[TF.name].ToString(),dict[TF.value]);
                }
            });

            return JsonConvert.SerializeObject(result);
        }

        string error() {
            JObject jo = new JObject();
            jo["msg"] = m_logic.Error;
            return JsonConvert.SerializeObject(jo);
        }

    }
}