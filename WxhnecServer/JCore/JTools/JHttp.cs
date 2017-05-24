using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace JCore
{
    public class JHttp
    {
        Stream m_stream;

        ~JHttp() {
            if(m_stream != null) {
                m_stream.Close();
            }
        }

        public bool GetData(string url, Dictionary<string, object> param = null) {
            var result = false;
            while (true) {
                if (string.IsNullOrEmpty(url)) {
                    break;
                }

                try {
                    url += THelper.Dict2QueryString(param);
                    var request = WebRequest.Create(url);
                    request.Method = "GET";
                    m_stream = request.GetResponse().GetResponseStream();
                    result = true;
                }
                catch { }

                break;
            }
            return result;
        }

        public bool PostData(string url, Dictionary<string, object> param = null, bool isJson = false) {
            var result = false;
            while (true) {
                if (string.IsNullOrEmpty(url)) {
                    break;
                }

                try {
                    // init
                    var request = WebRequest.Create(url);
                    request.Method = "POST";

                    // param
                    string options = null;
                    if (isJson) {
                        options = JsonConvert.SerializeObject(param);
                    }
                    else {
                        options = THelper.Dict2QueryString(param);
                        request.ContentType = "application/x-www-form-urlencoded";
                    }
                    byte[] data = Encoding.UTF8.GetBytes(options);
                    request.ContentLength = data.Length;

                    // request
                    Stream streamWriter = request.GetRequestStream();
                    streamWriter.Write(data, 0, data.Length);

                    // response
                    m_stream = request.GetResponse().GetResponseStream();
                    result = true;
                }
                catch { }

                break;
            }
            return result;
        }

        public string ToText() {
            var result = "";
            while (true) {
                if (m_stream == null) {
                    break;
                }

                StreamReader streamReader = null;
                try {
                    streamReader = new StreamReader(m_stream);
                    result = streamReader.ReadToEnd();
                }
                finally {
                    if (streamReader != null) {
                        streamReader.Close();
                    }
                }
                break;
            }
            return result;
        }

        public bool ToFile(string path) {
            var result = false;
            while (true) {
                if (m_stream == null) {
                    break;
                }

                if(!FileHelper.CheckDir(path, true)) {
                    break;
                }

                Stream fileStream = null;
                try {
                    fileStream = new FileStream(path, FileMode.Create);
                    const int c_length = 8 * 1024;
                    byte[] buffer = new byte[c_length];
                    int read;
                    while ((read = m_stream.Read(buffer, 0, c_length)) > 0) {
                        fileStream.Write(buffer, 0, read);
                    }
                    result = true;
                }
                finally {
                    if (fileStream != null) {
                        fileStream.Close();
                    }
                }
                break;
            }
            return result;
        }
    }
}