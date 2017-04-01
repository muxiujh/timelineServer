namespace JCore
{
    //
    // Summary:
    //      PageNode struct
    //
    public struct SPageNode
    {
        public SPageNode(int number, string text, bool isActive = false) {
            Number = number;
            Text = text;
            IsActive = isActive;
        }

        public int Number;
        public string Text;
        public bool IsActive;
    }
}