using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using WxhnecServer.Logics.Enums;

namespace WxhnecServer.Logics.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TValidate : ValidationAttribute
    {
        public TV key { get; set; }
        public object val { get; set; }
        static Dictionary<TV, TVConfig> config = getTVConfig();

        private object m_typeid;
        public override object TypeId {
            get {
                if (m_typeid == null) {
                    m_typeid = new object();
                }
                return m_typeid;
            }
        }

        public TValidate(TV pKey, object pVal = null) {
            key = pKey;
            val = pVal;
        }

        public override string FormatErrorMessage(string name) {
            TVConfig tvAttr = config[key];
            string result = tvAttr.err;
            switch (key) {
                case TV.minlength:
                case TV.maxlength:
                    result = string.Format(result, val);
                    break;
            }
            return result;
        }

        public override bool IsValid(object obj) {
            if (obj == null) {
                return true;
            }

            bool result = true;
            switch (key) {
                case TV.required:
                    Type type = obj.GetType();
                    if (type == typeof(String)) {
                        result = obj.ToString() != "";
                    }
                    else if (type == typeof(Int32)) {
                        result = Convert.ToInt32(obj) != 0;
                    }
                    else if (type == typeof(Int64)) {
                        result = Convert.ToInt64(obj) != 0;
                    }
                    break;
                case TV.minlength:
                    result = obj.ToString().Length >= Convert.ToInt32(val);
                    break;
                case TV.maxlength:
                    result = obj.ToString().Length <= Convert.ToInt32(val);
                    break;
            }
            TVConfig tvAttr = config[key];
            if (tvAttr.reg != null) {
                result = Regex.Match(obj.ToString(), tvAttr.reg).Success;
            }

            return result;
        }

        static Dictionary<TV, TVConfig> getTVConfig() {
            Dictionary<TV, TVConfig> dict = new Dictionary<TV, TVConfig>();
            FieldInfo[] fieldList = typeof(TV).GetFields();
            foreach (FieldInfo field in fieldList) {
                TVConfig tvAttr = field.GetCustomAttribute<TVConfig>();
                if(tvAttr == null) {
                    continue;
                }
                TV tv = (TV)Enum.Parse(typeof(TV), field.Name);
                dict[tv] = tvAttr;
            }
            return dict;
        }

    }

}