using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;

namespace JCore
{
    public class PropertyHelper
    {
        static public bool IsKey(PropertyInfo pro) {
            return pro.Name == "id";
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
                dict.TryGetValue(key, out temp);
                if (temp == null) {
                    dict[key] = null;
                }
            }
        }
    }
}