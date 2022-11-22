using CaffDal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CaffWeb.Pages
{
    public class UploadModel : PageModel
    {
        private readonly CaffFacade caffFacade;

        public UploadModel(CaffFacade caffFacade) {
            this.caffFacade = caffFacade;
        }


        public void OnGet()
        {
        }

        [Required(ErrorMessage = "Caff fájl megadása kötelezõ.")]
        [BindProperty]
        public IFormFile UploadaedCaff { get; set; }

        [Required(ErrorMessage = "A Caff név megadása kötelezõ.")]
        [BindProperty]
        public string FileName { get; set; }

        public async Task<IActionResult> OnPost() {
            using var memoryStream = new MemoryStream();
            await UploadaedCaff.CopyToAsync(memoryStream);
            byte[] raw = memoryStream.ToArray();

            var result = await caffFacade.UploadCaff(new CaffDal.Domain.UploadRequest {
                OwnerId = User.GetUserId(),
                CaffName = FileName,
                RawCaff = raw
            });

            if (result != null) {
                return RedirectToPage("/Caff", new { id = result.CaffId });
            } else {
                return Page();
            }
        }
    }
}
