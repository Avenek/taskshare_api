using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API_project_system.ModelsDto;
using API_project_system.Services;

namespace API_project_system.Controllers
{
    [Route("api/userLog")]
    [ApiController]
    [Authorize]
    public class UserLogController : ControllerBase
    {
        private readonly IUserLogService userLogService;

        public UserLogController(IUserLogService userLogService)
        {
            this.userLogService = userLogService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Get([FromQuery]string type = "None", [FromQuery] long startTimestamp = 0, [FromQuery] long endTimestamp = 0, [FromQuery] int userId = 0)
        {
            if (endTimestamp == 0)
            {
                endTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
            var actions = userLogService.Get(type, startTimestamp, endTimestamp, userId);
            return Ok(actions);
        }

    }
}
