using System;
using System.Collections.Generic;

namespace JCore
{
    public class QrcodeLogic
    {
        string m_home;
        string m_path;
        int m_width;
        string m_serverDir;
        string m_ossUrl;

        const string c_append = ".true";
        const int c_perWidth = 43;
        string c_width;
        string c_path;

        public string Error { get; set; }

        public QrcodeLogic(string serverDir) {
            m_serverDir = serverDir;
            var config = G.Config["Qrcode"];
            m_home = config["home"];
            m_path = config["path"];
            m_width = THelper.StringToInt(config["width"]);
            c_width = config["c_width"];
            c_path = config["c_path"];

            var ossConfig = G.Config["AliOss"];
            m_ossUrl = string.Format(ossConfig["url"], ossConfig["bucket"], ossConfig["server"]);
        }

        public string GetQrcode(int id, Func<bool> func) {
            string result = null;
            while (true) {
                if (id <= 0) {
                    Error = G.L["qr_id"];
                    break;
                }

                string shortPath = string.Format(m_path, id);
                string path = m_serverDir + shortPath;
                string pathTrue = path + c_append;
                if (FileHelper.CheckFile(pathTrue)) {
                    result = m_ossUrl + shortPath;
                    break;
                }

                if (func != null) {
                    if (!func()) {
                        Error = G.L["qr_func"];
                        break;
                    }
                }

                string home = string.Format(m_home, id);
                if (!GenerateQrcode(home, path)) {
                    Error = G.L["qr_generate"];
                    break;
                }

                AliOssLogic aliOss = new AliOssLogic();
                if (!aliOss.Add(shortPath, path)) {
                    Error = G.L["qr_ossadd"];
                    break;
                }

                if (!FileHelper.CreateFile(pathTrue)) {
                    Error = G.L["qr_createFileTrue"];
                    break;
                }

                if (!FileHelper.DeleteFile(path)) {
                    Error = G.L["qr_deleteFile"];
                    break;
                }

                result = m_ossUrl + shortPath;
                break;
            }
            return result;
        }

        public bool GenerateQrcode(string home, string path) {
            if (string.IsNullOrEmpty(home) || string.IsNullOrEmpty(path)) {
                return false;
            }

            var dict = new Dictionary<string, object>();
            var weixin = new JWeixin();
            dict.Add(c_path, home);
            dict.Add(c_width, m_width * c_perWidth);
            return weixin.GetQrcode(path, dict);
        }

    }
}