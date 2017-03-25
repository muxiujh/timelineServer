namespace JCore
{
    //
    // Summary:
    //      Page struct
    //
    public struct SPage
    {
        public SPage(int index, int size, int total, int pageShow = 10) {
            Index = index;
            Size = size;
            Total = total;
            PageShow = pageShow;
        }

        public int Index;
        public int Size;
        public int Total;
        public int PageShow;
    }
}