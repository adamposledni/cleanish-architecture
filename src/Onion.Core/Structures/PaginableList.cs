using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onion.Core.Structures
{
    public class PaginableList<T>
    {
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }

        public IList<T> Data { get; set; }

        public PaginableList(IList<T> data, int totalItems, int pageSize, int currentPage, int totalPages)
        {
            Data = data;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalItems = totalItems;
            CurrentPage = currentPage;
        }

        public PaginableList<U> Transform<U>(IList<U> data)
        {
            return new PaginableList<U>(data, TotalItems, PageSize, CurrentPage, TotalPages);
        }
    }
}
