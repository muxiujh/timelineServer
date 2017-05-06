using System;

namespace JCore
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TListSize : Attribute
    {
        public int Key { get; set; }

        public TListSize(int key = 0) {
            Key = key;
        }
    }
}