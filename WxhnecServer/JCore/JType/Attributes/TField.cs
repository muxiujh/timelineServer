using System;

namespace JCore
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class TField : Attribute
    {
        public TF Key { get; set; }
        public object Value { get; set; }

        public TField(TF key, object value = null) {
            Key = key;
            Value = value;
        }
    }
}