using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxhnecServer.Models;
using System.Reflection;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using WxhnecServer.Logics.Enums;
using WxhnecServer.Logics.Attributes;
using System.Collections.Specialized;
using WxhnecServer.Tools;

namespace WxhnecServer.Logics
{
    public class TFieldLogic<T> where T : class
    {
        Type m_type;

        public Type TType { get { return m_type; } }

        public TFieldLogic() {
            m_type = typeof(T);
        }

        public PropertyInfo[] GetPropertyList() {
            return m_type.GetProperties();
        }

        public Dictionary<TF, object> Field2UI(PropertyInfo pro, object val) {

            IEnumerable<TField> attributes = pro.GetCustomAttributes<TField>();
            Dictionary<TF, object> dict = attributes.ToDictionary(p => p.key, p => p.val);
            TElement element = pro.GetCustomAttribute<TElement>();
            if (element == null) {
                element = new TElement(TE.text);
            }

            // Reserved
            if (dict.ContainsKey(TF.reserved)) {
                return null;
            }

            // PrimaryKey
            if (pro.Name == "id") {
                dict[TF.type] = TE.hidden;
            }

            // type
            fillDictionary(ref dict, TF.type, element.key);
            string view;
            switch (element.key) {
                case TE.text:
                case TE.password:
                case TE.url:
                case TE.tel:
                    view = TE.text.ToString();
                    break;
                default:
                    view = element.key.ToString();
                    break;
            }

            // dataSource
            fillDictionary(ref dict, TF.dataSource, element.val);

            // name
            dict[TF.name] = pro.Name;

            // value
            if ((TE)dict[TF.type] == TE.date) {
                val = Convert.ToDateTime(val).ToString("yyyy-MM-dd");
            }
            dict[TF.value] = val;

            // title
            fillDictionary(ref dict, TF.title, "");

            // unit
            fillDictionary(ref dict, TF.unit, "");

            // desc
            fillDictionary(ref dict, TF.desc, null);

            return dict;
        }

        void fillDictionary(ref Dictionary<TF, object> dict, TF key, object value) {
            object temp;
            dict.TryGetValue(key, out temp);
            if (temp == null) {
                dict[key] = value;
            }
        }

        public T UI2Row(FormCollection collection) {
            T row = (T)Activator.CreateInstance(m_type, null);
            PropertyInfo[] propertyList = m_type.GetProperties();
            foreach (PropertyInfo pro in propertyList) {
                if (!collection.AllKeys.Contains(pro.Name)) {
                    continue;
                }

                object val = collection[pro.Name];
                convertValue(pro.PropertyType, ref val);
                pro.SetValue(row, val);
            }
            return row;
        }


        public object UI2RowRaw(NameValueCollection collection, Type type) {
            var row = Activator.CreateInstance(type, null);
            PropertyInfo[] propertyList = type.GetProperties();
            foreach (PropertyInfo pro in propertyList) {

                if (THelper.IsVirtual(pro)) {
                    if (THelper.HasElement(pro)) {
                        var subRow = UI2RowRaw(collection, pro.PropertyType);
                        pro.SetValue(row, subRow);
                    }
                }
                else if (collection.AllKeys.Contains(pro.Name)) {
                    object val = collection[pro.Name];
                    convertValue(pro.PropertyType, ref val);
                    pro.SetValue(row, val);
                }
                    
            }
            return row;
        }

        void checkNum(ref object val) {
            if(val.ToString() == "") {
                val = 0;
            }
        }

        void convertValue(Type type, ref object val) {
            if (type == typeof(Int32?)) {
                checkNum(ref val);
                val = Convert.ToInt32(val);
            }
            else if (type == typeof(Int64?)) {
                checkNum(ref val);
                val = Convert.ToInt64(val);
            }
            else if (type == typeof(DateTime?)) {
                val = Convert.ToDateTime(val);
            }
        }

        public string PrintEntity(FormCollection collection) {
            T row = UI2Row(collection);
            string result = "";
            PropertyInfo[] propertys = m_type.GetProperties();
            foreach (PropertyInfo pro in propertys) {
                if (!collection.AllKeys.Contains(pro.Name)) {
                    continue;
                }
                result += pro.Name + " = " + pro.GetValue(row) + "<br>";
            }
            return result;
        }
    }
}
