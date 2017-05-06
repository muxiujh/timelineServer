using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace JCore
{
    public class TQueryLogic
    {
        string m_t;
        bool m_init;
        dynamic m_object;

        public string FullName { get; set; }
        public SPage Paging { get; set; }
        public string Error { get; set; }
        Dictionary<string, string> Errors { get; set; }

        public bool InitQuery(string nameSpace, string t)
        {
            m_init = false;
            while (true) {
                if (string.IsNullOrWhiteSpace(nameSpace)) {
                    Error = "namespace is need.";
                    break;
                }

                if (string.IsNullOrWhiteSpace(t)) {
                    Error = "t is need.";
                    break;
                }

                m_t = t;
                FullName = nameSpace + "." + t;
                Type type = Type.GetType(FullName);
                if (type == null) {
                    Error = "no type " + FullName;
                    break;
                }

                m_init = true;
                break;
            }
            return m_init;
        }

        public object GetList(int pageIndex, int pageSize = 0, bool isPaging = false) {
            if(!m_init) {
                return null;
            }

            m_object = getTList();

            object result = m_object.TLimit(pageSize, pageIndex).ToList();
            if (isPaging) {
                int total = m_object.Count();
                Paging = new SPage(pageIndex, pageSize, total);
            }

            return result;
        }

        public object GetRow(int id = 0) {
            if (!m_init) {
                return null;
            }

            m_object = getTEntity();

            object row = null;
            if (id > 0) {
                string detail = m_t + "_detail";
                if (m_object.TType.GetProperty(detail) == null) {
                    detail = null;
                }
                row = m_object.FindRow(id, detail);
            }
            else {
                row = Activator.CreateInstance(m_object.TType);
            }

            return row;
        }

        public bool SaveRow(NameValueCollection collection) {           
            if (!m_init) {
                return false;
            }

            var m_object = getTEntity();
            TEntityUI entityUI = new TEntityUI();

            var row = entityUI.UI2Row(collection, m_object.TType);
            var id = collection.Get("id");
            bool result = false;
            if (string.IsNullOrEmpty(id)) {
                result = m_object.Add(row) != null;
            }
            else {
                result = m_object.Modify(row);
            }

            if (!result) {
                Errors = m_object.Errors;
                var err = Errors.First();
                Error = err.Key + " " + err.Value; ;
            }

            return result;
        }

        public string GetTitle() {
            if (m_object == null) {
                return null;
            }

            return m_object.GetTitle();
        }

        dynamic getTEntity() {
            Type generic = typeof(TEntityLogic<>);
            return THelper.CreateInstance(generic, FullName);
        }

        dynamic getTList() {
            Type generic = typeof(TListLogic<>);
            return THelper.CreateInstance(generic, FullName);
        }

    }
}