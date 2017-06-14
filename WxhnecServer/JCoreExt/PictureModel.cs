using System;
using System.Linq;
using WxhnecServer;

namespace JCore
{
    using T = pre_picture;
    public class PictureModel : TEntityLogic<T>
    {
        public new T Row { get; set; }

        public bool AddPic(string md5, string path, int companyid) {
            Row = new T();
            Row.md5 = md5;
            Row.path = path;
            Row.ctime = DateTime.Now;
            Row.companyid = companyid;

            Row = Add(Row);
            return Row != null;
        }

        public bool RemovePic(string path) {
            if (string.IsNullOrEmpty(path)) {
                return false;
            }

            Row = m_dbset.Where(t => t.path == path).FirstOrDefault();
            return Remove(Row);
        }

        public bool FindPic(string md5) {
            if (string.IsNullOrEmpty(md5)) {
                return false;
            }

            Row = m_dbset.Where(t => t.md5 == md5).FirstOrDefault();
            return Row != null;
        }
    }
}