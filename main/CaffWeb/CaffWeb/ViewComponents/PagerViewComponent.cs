using Microsoft.AspNetCore.Mvc;
using System;

namespace CaffWeb.ViewComponents
{
    public class PagerViewComponent : ViewComponent
    {
        public class PagerSpecification
        {
            public int TotalCount { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int PagesToShow { get; set; }
            public int TotalPages { get; set; }
        }

        public IViewComponentResult Invoke(int pageSize, int pageNumber, int totalCount, int pagesToShow) {
            return View(new PagerSpecification {
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PagesToShow = pagesToShow,
                TotalPages = (int)Math.Ceiling((double)totalCount / (double)pageSize)
            });
        }
    }
}
