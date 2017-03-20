using System;

namespace WxhnecServer.Logics.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TListSearch : Attribute
    {
        public bool Enable { get; set; }

        public TListSearch(bool enable = true) {
            Enable = enable;
        }
    }
}