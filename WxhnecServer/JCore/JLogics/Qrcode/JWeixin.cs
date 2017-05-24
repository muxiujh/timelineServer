using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace JCore
{
    public class JWeixin
    {
        string m_urlToken;
        string m_urlQrcode;
        string m_token;
        string c_token;

        public JWeixin() {
            var config = G.Config["Weixin"];
            m_urlToken = string.Format(config["urlToken"], config["appid"], config["appsecret"]);
            m_urlQrcode = config["urlQrcode"];
            c_token = config["c_token"];
        }

        public bool GetToken() {
            var result = false;
            while (true) {
                var client = new JHttp();
                if (!client.GetData(m_urlToken)) {
                    break;
                }

                var text = client.ToText();
                var obj = JsonConvert.DeserializeObject(text) as JObject;
                if(obj == null || obj[c_token] == null) {
                    break;
                }

                m_token = obj[c_token].ToString();
                result = !string.IsNullOrEmpty(m_token);
                break;
            }
            return result;
        }

        public bool GetQrcode(string path, Dictionary<string, object> dict) {
            var result = false;
            while (true) {
                if (string.IsNullOrEmpty(m_token)) {
                    if (!GetToken()) {
                        break;
                    }
                }

                var client = new JHttp();
                var urlQrcode = string.Format(m_urlQrcode, m_token);
                if (!client.PostData(urlQrcode, dict, true)) {
                    break;
                }

                if (!client.ToFile(path)) {
                    break;
                }

                result = true;
                break;
            }
            return result;
        }
    }
}