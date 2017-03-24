using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                if (result is JObject) {
                    var iterator = (result as JObject).GetEnumerator();
                    while (iterator.MoveNext()) {
                        loadSubFirst(iterator.Current.Value, dir, sub, key);
                    }
                }
                else if (result is JArray) {
                    var iterator = (result as JArray).GetEnumerator();
                    while (iterator.MoveNext()) {
                        loadSubFirst(iterator.Current, dir, sub, key);
                    }
                }

                break;
            }

            return result;
        }

    }
}