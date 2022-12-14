using System;
using System.Collections;
using System.Collections.Generic;

namespace MeasuringServer.Model.Paging
{
    public class PagedList<T> 
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

        private int numberOfItem;
        public int NumberOfItem
        {
            get { return numberOfItem; }
            set { numberOfItem = value; }
        }

        public int Count { get { return list.Count; } }
        

        public int NumberOfPage
        {
            get
            {                
                return numberOfItem / pageSize + (numberOfItem % pageSize > 0 ? 1 : 0);
            }
        }

        public bool HavePrevius
        {
            get { return page > 0; }
        }

        public bool HaveNext
        {
            get { return page < NumberOfPage; }
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
            numberOfItem = 0;
            list = new List<T>();
        }        

        public void SetPageData(int page, int pageSize, int numberOfAllItem)
        {
            this.Page = page;
            this.PageSize = pageSize;
            this.NumberOfItem = numberOfAllItem;
        }

        public override string ToString()
        {
            return $"Paged list type: {typeof(T)}, number of page {page}, page size {pageSize}, number of item {numberOfItem}, number of page {NumberOfPage}";
        }
    }
}
