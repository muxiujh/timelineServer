using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace JCore
{
    public class TQueryLogic
    {
        string m_t;
        dynamic m_object;

        public string FullName;
        public SPage Paging;
        public string Error;
        public Dictionary<string, string> Errors;
        public Dictionary<string, SCompare> CompareDict;
        public Dictionary<string, object> PresetDict;
        public object Row;

        public bool InitQuery(string nameSpace, string t, bool isRow = false) {
            bool result = false;
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
                
                m_object = isRow ? getTEntity() : getTList();
                result = m_object != null;
                break;
            }
            return result;
        }

        public TQueryLogic Condition(string condition) {
            while (true) {
                if (m_object == null) {
                    break;
                }

                if (string.IsNullOrEmpty(condition)) {
                    break;
                }

                CompareDict = THelper.String2CompareDict(condition);
                foreach (var item in CompareDict) {
                    var compare = item.Value;
                    m_object.TWhere(compare.Key, compare.Value, compare.Operate);
                }

                break;
            }
            return this;
        }

        public object GetList(int pageIndex, int pageSize = 0, bool isPaging = false) {
            if (m_object == null) {
                return null;
            }

            if (PresetDict != null) {
                foreach (var item in PresetDict) {
                    m_object.TWhere(item.Key, item.Value, op.eq);
                }
            }

            object result = m_object.TLimit(pageSize, pageIndex).ToList();
            if (isPaging) {
                int total = m_object.Count();
                Paging = new SPage(pageIndex, pageSize, total);
            }

            return result;
        }

        public bool CheckRow(int id = 0, bool isDetail = false) {
            while (true) {
                Row = null;
                if (m_object == null) {
                    break;
                }

                if (id <= 0) {
                    break;
                }

                if (!isDetail) {
                    Row = m_object.FindRow(id);
                    break;
                }

                string detail = m_t + "_detail";
                if (m_object.TType.GetProperty(detail) == null) {
                    detail = null;
                }
                Row = m_object.FindRow(id, detail);
                break;
            }
            return Row != null;
        }

        public bool CreateRow() {
            if (m_object == null) {
                return false;
            }

            Row = Activator.CreateInstance(m_object.TType);
            return Row != null;
        }

        public bool SaveRow(NameValueCollection collection) {
            if (m_object == null) {
                return false;
            }

            TEntityUI entityUI = new TEntityUI();

            THelper.MergeCollection(collection, PresetDict);
            var row = entityUI.UI2Row(collection, m_object.TType);
            var id = collection.Get("id");
            bool result = false;
            if (string.IsNullOrEmpty(id)) {
                result = m_object.Add(row) != null;
            }
            else {
                m_object.DetachRow(Row);
                result = m_object.Modify(row);
            }

            if (!result) {
                Errors = m_object.Errors;
                var err = Errors.First();
                Error = err.Key + " " + err.Value; ;
            }

            return result;
        }

        public bool ValidateField(string name, object value) {
            bool result = true;
            while (true) {
                if (m_object == null) {
                    break;
                }

                if (m_object.ValidateField(name, value)) {
                    break;
                }

                Errors = m_object.Errors;
                result = false;
                break;
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