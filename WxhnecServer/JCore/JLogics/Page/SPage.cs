namespace JCore
{
    //
    // Summary:
    //      Page struct
    //
    public struct SPage
    {
        public SPage(int pageIndex, int itemsPerPage, int itemsTotal, int pageShow = 10) {
            PageIndex = pageIndex;
            ItemsPerPage = itemsPerPage;
            ItemsTotal = itemsTotal;
            PageShow = pageShow;
        }

        public int PageIndex;
        public int PageShow;
        public int ItemsPerPage;
        public int ItemsTotal;
    }
}