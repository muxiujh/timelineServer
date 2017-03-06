using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxhnecServer.Logics.Enums;

namespace WxhnecServer.Logics.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TField : Attribute
    {
        public TF key { get; set; }
        public object val { get; set; }

        public TField(TF pKey, object pVal = null) {
            key = pKey;
            val = pVal;
        }
    }
}