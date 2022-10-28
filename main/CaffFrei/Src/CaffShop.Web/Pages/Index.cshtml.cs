using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaffShop.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger) {
            _logger = logger;
        }

        public int AddResult { get; set; }

        public void OnGet() {
            AddResult = CaffParserWrapper.CaffParser.Add(1, 2);
        }

        [BindProperty]
        public IFormFile UploadaedCaff { get; set; }
        public async Task<IActionResult> OnPost() {
            using var memoryStream = new MemoryStream();
            await UploadaedCaff.CopyToAsync(memoryStream);
            var raw = memoryStream.ToArray();
            var metadata = CaffParserWrapper.CaffParser.ParseMeta(raw);
            return Page();
        }

    }
}