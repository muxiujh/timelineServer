using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        static public int StringToInt(object value, int defaultValue = 0) {
            int result = defaultValue;
            while (true) {
                if (value == null) {
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

        static public string[] SplitString(string content, string comma) {
            if (string.IsNullOrEmpty(content)) {
                return new string[] { "" };
            }

            char tempComma = '&';
            content = content.Replace(comma, tempComma.ToString());
            return content.Split(tempComma);
        }

        ///
        /// <param name="content">key1l_lvalue1l__lkey2l_lvalue2></param>
        ///
        static public Dictionary<string, SCompare> String2CompareDict(string content, string split1 = G.Split1, string split2 = G.Split2) {
            var result = new Dictionary<string, SCompare>();
            if (string.IsNullOrWhiteSpace(content)
                || string.IsNullOrEmpty(split1)
                || string.IsNullOrEmpty(split2)) {
                return result;
            }

            var list = SplitString(content, split1);
            foreach (var item in list) {
                var pair = SplitString(item, split2);
                var count = pair.Count();
                if(count < 2) {
                    continue;
                }
                if (string.IsNullOrEmpty(pair[0]) || string.IsNullOrEmpty(pair[1])) {
                    continue;
                }
                SCompare compare;
                compare.Key = pair[0];
                compare.Value = pair[1];
                compare.Operate = count < 3 ? op.eq : pair[2];
                compare.Title = null;
                result.Add(compare.Key, compare);
            }

            return result;
        }

        static public List<SCompare> GetFilter(List<SCompare> searchFields, Dictionary<string, SCompare> compareDict) {
            var result = new List<SCompare>();
            while (true) {
                if (searchFields == null || searchFields.Count <= 0) {
                    break;
                }

                if(compareDict == null) {
                    compareDict = new Dictionary<string, SCompare>();
                }

                foreach (var item in searchFields) {
                    SCompare compare;
                    if(!compareDict.TryGetValue(item.Key, out compare)) {
                        compare.Key = item.Key;
                        compare.Value = null;
                        compare.Operate = op.eq;
                    }
                    compare.Title = item.Title;
                    result.Add(compare);
                }
                break;
            }
            return result;
        }

        static public void MergeCollection(NameValueCollection collection, Dictionary<string, object> session) {
            if(collection == null || session == null) {
                return;
            }

            var keys = collection.AllKeys;
            foreach (var item in session) {
                if (keys.Contains(item.Key) && item.Value != null) {
                    collection[item.Key] = item.Value.ToString();
                }
            }
        }

        public static List<string> GetKeys<TValue>(Dictionary<string, TValue> dict) { 
            var list = new List<string>();
            if(dict == null) {
                return list;
            }

            foreach (var item in dict) {
                list.Add(item.Key);
            }
            return list;
        }

        static public string Lang(string title, string name = null) {
            string result = null;
            while (true) {
                if (string.IsNullOrEmpty(title)) {
                    if (string.IsNullOrEmpty(name)) {
                        break;
                    }
                    title = name;
                }

                result = G.L[title];
                break;
            }
            return result;
        }

        public static string Dict2QueryString<TValue>(Dictionary<string, TValue> dict, string comma = "") {
            var result = "";
            while (true) {
                if (dict == null) {
                    break;
                }

                foreach (var item in dict) {
                    result += comma + item.Key + "=" + item.Value;
                    comma = "&";
                }
                break;
            }
            return result;
        }

        public static string UpFirst(string content) {
            var result = "";
            while (true) {
                if (string.IsNullOrEmpty(content)) {
                    break;
                }

                result = content.Substring(0, 1).ToUpper() + content.Substring(1);
                break;
            }
            return result;
        }

    }
}