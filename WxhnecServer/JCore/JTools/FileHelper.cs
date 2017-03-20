using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JCore
{
    public class FileHelper
    {

        static public string GetPath(string dir, string name, string ext){
            return dir + name + "." + ext.ToLower();
        }

        static public string GetSizePath(string path, int size) {
            string name = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);
            string newName = String.Format("{0}_{1}{2}", name, size, ext);
            return path.Replace(name + ext, newName);
        }

        static public bool CheckFile(string file) {
            bool result = false;
            while (true) {
                if (file == null) {
                    break;
                }

                if (!File.Exists(file)) {
                    break;
                }

                result = true;
                break;
            }
            return result;
        }

        static public bool CheckDir(string dir, bool isFile = false) {
            if(dir == null) {
                return false;
            }

            if (isFile) {
                dir = Path.GetDirectoryName(dir);
            }

            bool result = true;
            try {
                if (!Directory.Exists(dir)) {
                    Directory.CreateDirectory(dir);
                }
            }
            catch {
                result = false;
            }
            return result;
        }

        static public string GetDateDir() {
            return DateTime.Now.ToString("yyyy/MM/dd/");
        }

        static public string GetRandomName() {
            string str = DateTime.Now.ToString("HH_mm_ss_");
            Random random = new Random();
            return str + random.Next();
        }

        static public bool CreateFile(string path) {
            if(!CheckDir(path, true)) {
                return false;
            }

            using (File.Create(path)) {
                ;
            }
            return true;
        }

        static public bool DeleteFile(string path) {
            if (path == null) {
                return true;
            }

            bool result = false;
            try {
                File.Delete(path);
                result = true;
            }
            catch { }
            return result;
        }

        static public string GetMD5(string path) {
            if (!CheckFile(path)) {
                return null;
            }

            string result = null;
            try {
                FileStream fs = new FileStream(path, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                var bytes = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes) {
                    sb.Append(b.ToString("x2"));
                }
                result = sb.ToString();
            }
            catch { }
            return result;
        }

        static public string SubMD5(string md5, int length = 16) {
            if(md5 == null) {
                return null;
            }

            if(md5.Length < length) {
                length = md5.Length;
            }

            return md5.Substring(0, length);
        }
    }
}