using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace JCore
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TValidate : ValidationAttribute
    {
        public TV Key { get; set; }
        public object Value { get; set; }

        private object m_typeid;
        public override object TypeId {
            get {
                if (m_typeid == null) {
                    m_typeid = new object();
                }
                return m_typeid;
            }
        }

        public TValidate(TV key, object value = null) {
            Key = key;
            Value = value;
        }

        public override string FormatErrorMessage(string name) {
            TVConfig tvAttr = TVConfig.Configs[Key];
            string result = tvAttr.Error;
            switch (Key) {
                case TV.minlength:
                case TV.maxlength:
                    result = string.Format(result, Value);
                    break;
            }
            return result;
        }

        public override bool IsValid(object obj) {
            if (obj == null) {
                return true;
            }

            bool result = true;
            switch (Key) {
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
                    result = obj.ToString().Length >= Convert.ToInt32(Value);
                    break;
                case TV.maxlength:
                    result = obj.ToString().Length <= Convert.ToInt32(Value);
                    break;
                default: {
                        TVConfig tvAttr = TVConfig.Configs[Key];
                        if (tvAttr.Regulation != null) {
                            result = Regex.Match(obj.ToString(), tvAttr.Regulation).Success;
                        }
                        break;
                    }
            }
            return result;
        }

    }

}