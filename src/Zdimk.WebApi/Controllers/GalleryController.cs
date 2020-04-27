using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Zdimk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GalleryController : ControllerBase
    {
        [HttpGet]
        public string Test()
        {
            return "Hello world";
        }
    }
}