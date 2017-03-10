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
        public TE Key { get; set; }
        public object Value { get; set; }

        public TElement(TE key, object value = null) {
            Key = key;
            Value = value;
        }
    }
}