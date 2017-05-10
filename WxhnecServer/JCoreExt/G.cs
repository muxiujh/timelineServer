using System.Collections.Generic;
using WxhnecServer;

namespace JCore
{
    public class G
    {
        static ConfigModel m_configModel = new ConfigModel();

        public static Dictionary<string, dynamic> Config {
            get {
                return m_configModel.Cache();
            }
        }

        public const string Split1 = "l__l";
        public const string Split2 = "l_l";
    }
}