using Newtonsoft.Json.Linq;

namespace JCore
{
    public class JLang
    {
        JObject m_jo;
        public bool Success { get { return m_jo != null; } }

        public JLang(string path) {
            if (!string.IsNullOrEmpty(path)) {
                m_jo = ConfigHelper.LoadConfig(path) as JObject;
            }
        }

        public string this[string key] {
            get {
                var result = key;
                while (true) {
                    if (string.IsNullOrEmpty(key) || m_jo == null || m_jo[key] == null) {
                        break;
                    }

                    result = m_jo[key].ToString();
                    break;
                }
                return result;
            }
        }
    }
}