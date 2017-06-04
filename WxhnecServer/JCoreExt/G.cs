using System.Collections.Generic;
using WxhnecServer;

namespace JCore
{
    using TDict = Dictionary<string, List<string>>;
    public static class G
    {
        static G() {
            string path = "D:/Project/ASP.NET/timelineServer/WxhnecServer/Resources/lang/cn.json";
            L = new JLang(path);
        }

        static ConfigModel m_configModel = new ConfigModel();
        public static Dictionary<string, dynamic> Config {
            get {
                return m_configModel.Cache();
            }
        }

        public const string Split1 = "l__l";
        public const string Split2 = "l_l";
        public const string companyid = "companyid";
        public const string super = "super";

        public static JLang L;
    }
}