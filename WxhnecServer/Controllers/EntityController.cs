using JCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WxhnecServer
{
    public class EntityController : BaseController
    {
        TQueryLogic m_logic = new TQueryLogic();
        string m_namespace { get { return GetType().Namespace; } }
        protected bool m_json = true;

        [HttpGet]
        public object List(string t = null, string c = null, int p = 1) {
            if (!m_logic.InitQuery(m_namespace, t)) {
                return error();
            }

            var result = m_logic.Condition(c).GetList(p);

            return toJson(result);
        }

        [HttpGet]
        public object Row(int id = 0, string t = null) {
            if (!m_logic.InitQuery(m_namespace, t, true)) {
                return error();
            }

            if(!m_logic.CheckRow(id, true)) {
                return error();
            }

            TEntityUI m_entityUI = new TEntityUI();
            var result = new Dictionary<string, object>();
            m_entityUI.Row2UI(m_logic.Row, dict => {
                if (dict != null) {
                    result.Add(dict[TF.name].ToString(),dict[TF.value]);
                }
            });

            return toJson(result);
        }
        

        object error() {
            JObject jo = new JObject();
            jo["msg"] = m_logic.Error;
            return toJson(jo);
        }

        protected object toJson(object obj) {
            if(m_json == true) {
                return JsonConvert.SerializeObject(obj);
            }
            else {
                return obj;
            }
        }

    }
}