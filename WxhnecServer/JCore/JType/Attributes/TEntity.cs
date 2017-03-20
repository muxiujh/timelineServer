using System;

namespace JCore
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