using CaffDal.Domain;
using CaffDal.Domain.Pager;
using CaffDal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaffWeb.Pages
{
    public class CaffShopModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public PagedSearchSpecification Specification { get; set; }

        public PagedResult<CompactPreviewResponse> Caffs { get; set; }

        private readonly ICaffFacade caffFacade;

        public CaffShopModel(ICaffFacade caffFacade) {
            this.caffFacade = caffFacade;
        }

        public async Task OnGetAsync()
        {
            Caffs = await caffFacade.PagedSearch(Specification);
        }
    }
}
