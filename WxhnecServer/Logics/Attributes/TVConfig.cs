using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using WxhnecServer.Logics.Enums;
using WxhnecServer.Tools;

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

        static public Dictionary<TV, TVConfig> Configs = THelper.GetEnumConfigs<TV, TVConfig>();

    }
}