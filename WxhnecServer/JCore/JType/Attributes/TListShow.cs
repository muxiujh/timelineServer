using System;

namespace JCore
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