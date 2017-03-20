using Aliyun.OSS;
using System;

namespace JCore
{
    public class AliOssLogic
    {
        OssClient m_client;
        string m_bucket;
        public AliOssLogic() {
            string endpoint = "oss-cn-hangzhou.aliyuncs.com";
            string accessKeyId = "yL9aubzdQnPA2S9Z";
            string accessKeySecret = "6eqg815UcfGsFXSfhmBoWtIDXkfuT4";

            m_client = new OssClient(endpoint, accessKeyId, accessKeySecret);
            m_bucket = "wxhneclocal";
        }

        public bool Add(string key, string path) {
            if (string.IsNullOrEmpty(key)) {
                return false;
            }

            if (!FileHelper.CheckFile(path)) {
                return false;
            }

            return run(() => {
                var result = m_client.PutObject(m_bucket, key, path);
            });
        }

        public bool Remove(string key) {
            if (string.IsNullOrEmpty(key)) {
                return false;
            }

            return run(() => {
                m_client.DeleteObject(m_bucket, key);
            });
        }

        public bool Find(string key) {
            if (string.IsNullOrEmpty(key)) {
                return false;
            }

            return run(() => {
                var result = m_client.GetObject(m_bucket, key);
            });
        }

        bool run(Action act) {
            bool result = false;
            try {
                act();
                result = true;
            }
            catch { }
            return result;
        }
    }
}