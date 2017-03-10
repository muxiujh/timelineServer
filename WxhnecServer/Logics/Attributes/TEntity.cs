using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxhnecServer.Logics.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TEntity : Attribute
    {
        public string Value { get; set; }

        public TEntity(string value) {
            Value = value;
        }
    }

}