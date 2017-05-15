using System.Collections.Generic;
using WxhnecServer;

namespace JCore
{
    using TDict = Dictionary<string, List<string>>;
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

        public const string companyid = "companyid";
        public const string super = "super";

        public static TDict Preset {
            get {
                var dict = new TDict();
                dict.Add(companyid, new List<string>() { nameof(pre_product), nameof(pre_news) });
                return dict;
            }
        }

        public static TDict PresetId {
            get {
                var dict = new TDict();
                dict.Add(companyid, new List<string>() { nameof(pre_company)});
                return dict;
            }
        }
    }
}