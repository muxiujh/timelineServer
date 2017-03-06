using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WxhnecServer.Logics.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TEntity : Attribute
    {
        public string Name { get; set; }

        public TEntity(string name) {
            Name = name;
        }
    }

}