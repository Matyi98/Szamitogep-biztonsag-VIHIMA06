using CaffDal.Services;
using Microsoft.AspNetCore.Mvc;

namespace CaffWeb.Controllers
{
    [Route("/images")]
    [ApiController]
    public class ImageController : Controller
    {
        private readonly ICaffFacade caffFacade;
        public ImageController(ICaffFacade caffFacade) {
            this.caffFacade = caffFacade;
        }

        public async Task<ActionResult> GetImageAsync(int id) {
            byte[] image = await caffFacade.GetImage(id);
            return File(image, "image/jpg");
        }


    }
}
