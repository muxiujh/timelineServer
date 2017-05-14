namespace JCore
{
    public struct SCompare
    {
        public SCompare(string key, object value, string operate, string title = null) {
            Key = key;
            Value = value;
            Operate = operate;
            Title = title;
        }

        public string Key;
        public object Value;
        public string Operate;
        public string Title;
    }

}