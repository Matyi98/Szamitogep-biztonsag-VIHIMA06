using CaffDal.Services;
using Microsoft.AspNetCore.Mvc;

namespace CaffWeb.Controllers
{
    [Route("/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ICaffFacade caffFacade;
        public ImageController(ICaffFacade caffFacade)
        {
            this.caffFacade = caffFacade;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetImageAsync([FromRoute] int id)
        {

            byte[] image = await caffFacade.GetImage(id);
            return File(image, "image/jpg");
        }


    }
}
