namespace JCore
{
    public struct SCompare
    {
        public SCompare(string key, string value, string operate, string title = null) {
            Key = key;
            Value = value;
            Operate = operate;
            Title = title;
        }

        public string Key;
        public string Value;
        public string Operate;
        public string Title;
    }

}