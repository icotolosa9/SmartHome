using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Out
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Results { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        public PagedResult(IEnumerable<T> results, int totalCount, int pageNumber, int pageSize)
        {
            Results = results;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
