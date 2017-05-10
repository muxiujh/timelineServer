namespace JCore
{
    public struct SCompare
    {
        public SCompare(string key, string value, string operate) {
            Key = key;
            Value = value;
            Operate = operate;
        }

        public string Key;
        public string Value;
        public string Operate;
    }

}