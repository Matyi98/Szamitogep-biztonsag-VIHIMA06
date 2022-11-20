using System.Collections.Generic;

namespace CaffDal.Domain.Pager
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Results { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
