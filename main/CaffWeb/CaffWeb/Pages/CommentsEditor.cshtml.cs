using CaffDal.Domain;
using CaffDal.Identity;
using CaffDal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CaffWeb.Pages
{
    public class CommentsEditorModel : PageModel
    {
        private readonly ICaffFacade caffFacade;

        public CommentsEditorModel(ICaffFacade caffFacade) {
            this.caffFacade = caffFacade;
        }


        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CaffId { get; set; }


        [BindProperty]
        [Required(ErrorMessage = "Üres komment nem írható!")]
        [MinLength(1)]
        public string Edited { get; set; }

        public async Task<ActionResult> OnGetAsync()
        {
            CommentResponse comment;
            try {
                comment = await caffFacade.GetCommentById(Id);
                Edited = comment.Text;
            } catch (Exception) {
                return NotFound($"Nem található Comment a megadott Id-val: '{Id}'.");
            }

            if (!User.IsInRole(Roles.Admin) && User.GetUserId() != comment.CommenterId)
                return Redirect("/Identity/Account/AccessDenied");

            return Page();
        }

        public async Task<ActionResult> OnPostDeleteAsync() {
            CommentResponse comment;
            try {
                comment = await caffFacade.GetCommentById(Id);
                Edited = comment.Text;
            } catch (Exception) {
                return NotFound($"Nem található Comment a megadott Id-val: '{Id}'.");
            }

            if (!User.IsInRole(Roles.Admin) && User.GetUserId() != comment.CommenterId)
                return Redirect("/Identity/Account/AccessDenied");

            await caffFacade.DeleteComment(Id);


            return RedirectToPage("/Caff", new { id = CaffId });
        }

        public async Task<ActionResult> OnPostEditAsync() {
            CommentResponse comment;
            try {
                comment = await caffFacade.GetCommentById(Id);
            } catch (Exception) {
                return NotFound($"Nem található Comment a megadott Id-val: '{Id}'.");
            }

            if (User.GetUserId() != comment.CommenterId)
                return Redirect("/Identity/Account/AccessDenied");

            await caffFacade.ModifyComment(Id, Edited);

            return RedirectToPage("/Caff", new { id = CaffId });
        }
    }
}
