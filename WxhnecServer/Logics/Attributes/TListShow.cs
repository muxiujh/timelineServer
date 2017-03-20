using System;

namespace WxhnecServer.Logics.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class TListShow : Attribute
    {
        public bool Enable { get; set; }

        public TListShow(bool enable = true) {
            Enable = enable;
        }
    }
}