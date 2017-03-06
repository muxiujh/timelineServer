using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Reflection;
using WxhnecServer.Logics.Attributes;

namespace WxhnecServer.Tools
{
    public class THelper
    {
        static public Dictionary<int, string> getList(string key) {
            Dictionary<int, string> list = new Dictionary<int, string> {
                { 1, "yes" },
                { 2, "no" }
            };
            return list;
        }

        static public bool IsVirtual(PropertyInfo pro) {
            return pro.GetMethod.IsVirtual;
        }
        static public bool HasElement(PropertyInfo pro) {
            return pro.GetCustomAttribute<TElement>() != null;
        }
    }
}