using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WxhnecServer.Logics.Enums;

namespace WxhnecServer.Logics.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TElement : Attribute
    {
        public TE key { get; set; }
        public object val { get; set; }

        public TElement(TE pKey, object pVal = null) {
            key = pKey;
            val = pVal;
        }
    }
}