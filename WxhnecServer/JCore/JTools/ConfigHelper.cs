using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace JCore
{
    public class ConfigHelper
    {

        static public object LoadConfig(string path) {
            object result = null;
            while (true) {
                if (!FileHelper.CheckFile(path)) {
                    break;
                }

                string temp = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(temp)) {
                    break;
                }

                try {
                    result = JsonConvert.DeserializeObject(temp);
                }
                catch { }

                break;
            }

            return result;
        }

        static void loadSubFirst(JToken item, string dir, string sub, string key) {
            while (true) {
                // try subName
                var subName = item[sub];
                if (subName == null) {
                    break;
                }

                // try load subConfig
                var subConfig = LoadConfig(dir + "/" + subName);
                if (subConfig == null) {
                    break;
                }

                if (subConfig is JArray) {
                    item[key] = (subConfig as JArray).First;
                }
                else if (subConfig is JObject) {
                    item[key] = (subConfig as JObject).First.First;
                }

                break;
            }
        }

        static public object LoadConfigIncludeSubFirst(string path, string sub, string key) {
            object result = null;
            while (true) {
                result = LoadConfig(path);
                if (result == null) {
                    break;
                }

                if (string.IsNullOrEmpty(sub) || string.IsNullOrEmpty(key)) {
                    break;
                }

                string dir = Path.GetDirectoryName(path);
                JForeach(result, node => {
                    loadSubFirst(node, dir, sub, key);
                });

                break;
            }

            return result;
        }

        static public List<string> GetList(object obj, string key) {
            var result = new List<string>();
            JForeach(obj, node => {
                var item = node[key];
                if (item != null) {
                    result.Add(item.ToString());
                }
            });
            return result;
        }

        static void JForeach(object obj, Action<JToken> act) {
            if(obj == null || act == null) {
                return;
            }

            if (obj is JObject) {
                var iterator = (obj as JObject).GetEnumerator();
                while (iterator.MoveNext()) {
                    act(iterator.Current.Value);
                }
            }
            else if (obj is JArray) {
                var iterator = (obj as JArray).GetEnumerator();
                while (iterator.MoveNext()) {
                    act(iterator.Current);
                }
            }
        }


    }
}