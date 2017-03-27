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
    }
}