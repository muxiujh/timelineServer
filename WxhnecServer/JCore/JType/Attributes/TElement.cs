using System;

namespace JCore
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