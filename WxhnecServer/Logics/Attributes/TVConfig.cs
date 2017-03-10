using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WxhnecServer.Logics.Enums;

namespace WxhnecServer.Logics.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TVConfig: Attribute
    {
        public string Error { get; set; }
        public string Regulation { get; set; }

        public TVConfig(string error, string regulation = null) {
            Error = error;
            Regulation = regulation;
        }
        
        static public Dictionary<TV, TVConfig> Configs = getTVConfigs();

        static private Dictionary<TV, TVConfig> getTVConfigs() {
            Dictionary<TV, TVConfig> dict = new Dictionary<TV, TVConfig>();
            FieldInfo[] fieldList = typeof(TV).GetFields();
            foreach (FieldInfo field in fieldList) {
                TVConfig tvAttr = field.GetCustomAttribute<TVConfig>();
                if (tvAttr == null) {
                    continue;
                }
                TV tv = (TV)Enum.Parse(typeof(TV), field.Name);
                dict[tv] = tvAttr;
            }
            return dict;
        }
    }
}