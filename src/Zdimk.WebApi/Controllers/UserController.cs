using Microsoft.AspNetCore.Mvc;
using Zdimk.Services;

namespace Zdimk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        public UserController(PictureService service)
        {
            
        }
    }
}