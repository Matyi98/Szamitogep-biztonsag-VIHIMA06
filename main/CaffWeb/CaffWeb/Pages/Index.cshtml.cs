using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CaffWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger) {
            _logger = logger;
        }

        public void OnGet() {
            
        }

        [BindProperty]
        public IFormFile UploadaedCaff { get; set; }
        public async Task<IActionResult> OnPost() {
            using var memoryStream = new MemoryStream();
            await UploadaedCaff.CopyToAsync(memoryStream);
            byte[] raw = memoryStream.ToArray();

            //TODO

            return Page();
        }

    }
}