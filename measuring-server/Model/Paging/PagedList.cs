using System.Collections;
using System.Collections.Generic;

namespace MeasuringServer.Model.Paging
{
    public abstract class PagedList<T> 
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

        private int numberOfPage;
        public int NumberOfPage
        {
            get { return numberOfPage; }
            set { numberOfPage = value; }
        }

        List<T> list;
        public List<T> List
        {
            get { return list; }
            set { list = value; }
        }

        public PagedList()
        {
            page = 0;
            pageSize = 0;
            numberOfPage = 0;
            list = new List<T>();
        }
    }
}
