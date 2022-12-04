using CaffDal.Domain;
using CaffDal.Identity;
using CaffDal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CaffWeb.Pages
{
    public class CaffModel : PageModel
    {
        private readonly ICaffFacade caffFacade;

        public CaffModel(ICaffFacade caffFacade)
        {
            this.caffFacade = caffFacade;
        }


        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }


        [BindProperty]
        [Required(ErrorMessage = "Üres komment nem írható!")]
        [MinLength(1)]
        public string Comment { get; set; }

        public IReadOnlyCollection<CommentResponse> Comments { get; set; }
        public DetailedPreviewResponse Caff { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Caff = await caffFacade.GetPreview(Id);
                Comments = await caffFacade.GetComments(Id);
            }
            catch (Exception)
            {
                return NotFound($"Nem található CAFF a megadott Id-val: '{Id}'.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteCaffAsync()
        {
            if (!User.IsInRole(Roles.Admin) && User.GetUserId() != Caff.CreatorID)
                return Redirect("/Identity/Account/AccessDenied");

            await caffFacade.DeleteCaff(Id);
            return RedirectToPage("/Index");
        }


        public async Task<IActionResult> OnPostWriteCommentAsync()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/Identity/Account/AccessDenied");

            await caffFacade.WriteComment(Id, Comment, User.GetUserId());
            return RedirectToPage("/Caff", new { id = Id });
        }

        public async Task<IActionResult> OnPostPurchaseAsync()
        {
            var caff = await caffFacade.BuyCaff(Id);
            return File(caff.Bytes, "application/octet-stream", $"{caff.Name}.caff");
        }

    }
}
