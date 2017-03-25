namespace JCore
{
    //
    // Summary:
    //      PageNode struct
    //
    public struct SPageNode
    {
        public SPageNode(int index, string text, bool active = false) {
            Index = index;
            Text = text;
            Active = active;
        }

        public int Index;
        public string Text;
        public bool Active;
    }
}