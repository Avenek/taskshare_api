using Microsoft.AspNetCore.Mvc;
using API_project_system.ModelsDto;
using API_project_system.Services;

namespace API_project_system.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountControler : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountControler(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            accountService.RegisterUser(registerUserDto);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto loginUserDto)
        {
            string token = accountService.TryLoginUserAndGenerateJwt(loginUserDto);
            return Ok(token);
        }

        [HttpPost("{userId}")]
        public ActionResult UpdateUser(int userId, [FromBody] UpdateUserDto updateUserDto) 
        {
            accountService.UpdateUser(userId, updateUserDto);
            return Ok();
        }
    }
}
