namespace MeasuringServer.Model.Paging
{
    public class PagingData
    {
        private int page;
        public int Page
        {
            get { return page; }
            set { page = value; }
        }

        private int pageSize;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
    }
}
