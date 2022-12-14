using CaffDal.Domain.Pager;

namespace CaffDal.Domain
{
    public interface ISearchSpecification
    {
        public string? Creator { get; set; }
        public DateTime? CreationDateStart { get; set; }
        public DateTime? CreationDateEnd { get; set; }
        public string? Name { get; set; }
    }


    public class PagedSearchSpecification : PagerSpecification, ISearchSpecification
    {
        public string? Creator { get; set; }
        public DateTime? CreationDateStart { get; set; }
        public DateTime? CreationDateEnd { get; set; }
        public string? Name { get; set; }
    }
}
