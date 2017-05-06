using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JCore
{
    public class THelper
    {
        static public T ConvertToEnum<T>(string key, T defaultEnum) {
            T result = defaultEnum;
            while (true) {
                if (string.IsNullOrWhiteSpace(key)) {
                    break;
                }

                try {
                    result = (T)Enum.Parse(typeof(T), key);
                }
                catch { }

                break;
            }
            return result;
        }

        static public Dictionary<TEnum, TConfig> GetEnumConfigs<TEnum, TConfig>() where TConfig: Attribute {
            Dictionary<TEnum, TConfig> dict = new Dictionary<TEnum, TConfig>();
            FieldInfo[] fieldList = typeof(TEnum).GetFields();
            foreach (FieldInfo field in fieldList) {
                Type type = typeof(TConfig);
                TConfig tConfig = field.GetCustomAttribute<TConfig>();
                if (tConfig == null) {
                    continue;
                }
                TEnum tEnum = (TEnum)Enum.Parse(typeof(TEnum), field.Name);
                dict[tEnum] = tConfig;
            }
            return dict;
        }

        static public Type GetBaseType(object obj) {
            if(obj == null) {
                return null;
            }
            
            Type type = obj.GetType();
            if(type.BaseType != typeof(Object)) {
                type = type.BaseType;
            }

            return type;
        }

        static Random m_random = new Random();
        static public int GetRandom(int min = 1, int max = 100) {
            return m_random.Next(min, max);
        }


        static public void ConvertToUI(TE key, ref object value) {
            if (value == null) {
                return;
            }

            switch (key) {
                case TE.date:
                    DateToString(ref value);
                    break;
            }
        }

        static public void DateToString(ref object value) {
            if (value == null) {
                return;
            }

            value = Convert.ToDateTime(value).ToString("yyyy-MM-dd");
        }

        static public void ConvertToZero(ref object value) {
            if(value == null) {
                return;
            }

            if (value.ToString() == "") {
                value = 0;
            }
        }

        static public void ConvertToType(Type type, ref object value) {
            if (value == null) {
                return;
            }

            try {
                if (type == typeof(Int32?)) {
                    ConvertToZero(ref value);
                    value = Convert.ToInt32(value);
                }
                else if (type == typeof(Int64?)) {
                    ConvertToZero(ref value);
                    value = Convert.ToInt64(value);
                }
                else if (type == typeof(DateTime?)) {
                    value = Convert.ToDateTime(value);
                }
            }
            catch (FormatException) {
                value = null;
            }
        }

        static public int StringToInt(string value, int defaultValue = 0) {
            int result = defaultValue;
            while (true) {
                if (string.IsNullOrWhiteSpace(value)) {
                    break;
                }
                try {
                    result = Convert.ToInt32(value);
                }
                catch { }

                break;
            }
            return result;
        }
        
        static public object CreateInstance(Type generic, string tNamespaceClass) {
            object result = null;
            try {
                Type entityType = Type.GetType(tNamespaceClass, true);
                Type[] typeArgs = { entityType };
                var gen = generic.MakeGenericType(typeArgs);
                result = Activator.CreateInstance(gen, null);
            }
            catch { }
            return result;
        }

        static public string GetFirstString(string content) {
            return GetFirstStringRaw(content, ',');
        }

        static public string GetFirstStringRaw(string content, char comma) {
            if (string.IsNullOrWhiteSpace(content)) {
                return "";
            }

            var arr = content.Split(comma);
            return arr.First();
        }
    }
}