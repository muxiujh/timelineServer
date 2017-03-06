using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxhnecServer.Logics.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class TVConfig: Attribute
    {
        public string err { get; set; }
        public string reg { get; set; }

        public TVConfig(string pErr, string pReg = null) {
            err = pErr;
            reg = pReg;
        }

    }
}