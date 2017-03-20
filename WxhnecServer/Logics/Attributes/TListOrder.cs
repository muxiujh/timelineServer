using System;

namespace WxhnecServer.Logics.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TListOrder : Attribute
    {
        public string Key { get; set; }
        public bool IsAsc { get; set; }

        public TListOrder(string key, bool isAsc = true) {
            Key = key;
            IsAsc = isAsc;
        }
    }
}