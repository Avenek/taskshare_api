using Microsoft.AspNetCore.Mvc;
using API_project_system.Services;

namespace API_project_system.Controllers
{
    [Route("api/user")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok();
        }
    }
}
