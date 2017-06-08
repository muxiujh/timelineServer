using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCore
{
    public class PropertyHelper
    {
        public const string DefaultKey = "id";

        static public bool IsKey(PropertyInfo pro) {
            return pro.Name == DefaultKey;
        }

        static public bool IsVirtual(PropertyInfo pro) {
            return pro.GetMethod.IsVirtual;
        }

        static public bool HasElement(PropertyInfo pro) {
            return pro.GetCustomAttribute<TElement>() != null;
        }

        static public bool IsAutoIncrease(PropertyInfo pro) {
            bool result = true;
            DatabaseGeneratedAttribute attr = pro.GetCustomAttribute<DatabaseGeneratedAttribute>();
            if(attr != null && attr.DatabaseGeneratedOption == DatabaseGeneratedOption.None) {
                result = false;
            }
            return result;
        }

        static public bool IsRequired(PropertyInfo pro) {
            bool bResult = false;
            var attrs = pro.GetCustomAttributes<TValidate>();
            foreach (TValidate tv in attrs) {
                if (tv.Key == TV.required) {
                    bResult = true;
                    break;
                }
            }
            return bResult;
        }

        static public void FillTFDictionary(ref Dictionary<TF, object> dict) {
            if (dict == null) {
                return;
            }

            FieldInfo[] fieldList = typeof(TF).GetFields();
            Type type = typeof(TF);
            foreach (FieldInfo field in fieldList) {
                if (field.FieldType != type) {
                    continue;
                }

                TF key = (TF)Enum.Parse(type, field.Name);
                object temp;
                if (!dict.TryGetValue(key, out temp)) {
                    dict[key] = null;
                }
            }
        }

        static public bool HasAttribute<TAttribute>(PropertyInfo pro) where TAttribute : Attribute {
            bool result = false;
            while (true) {
                var attr = pro.GetCustomAttribute<TAttribute>();
                if (attr == null) {
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

        static public string GetTFieldValue(PropertyInfo pro, TF key) {
            string result = null;
            while (true) {
                var value = GetTFieldValueRaw(pro, key);
                if (value == null) {
                    break;
                }

                result = value.ToString();
                break;
            }
            return result;
        }

        static public object GetTFieldValueRaw(PropertyInfo pro, TF key) {
            object result = null;
            while (true) {
                var fieldList = pro.GetCustomAttributes<TField>();
                if (fieldList == null) {
                    break;
                }

                foreach (TField field in fieldList) {
                    if (field.Key == key) {
                        result = field.Value;
                        break;
                    }
                }

                break;
            }
            return result;
        }

        static public bool IsElement(PropertyInfo pro, TE key) {
            bool result = false;
            while (true) {
                var element = pro.GetCustomAttribute<TElement>();
                if (element == null) {
                    break;
                }

                if(element.Key == key) {
                    result = true;
                    break;
                }

                break;
            }
            return result;
        }
    }
}