using Microsoft.AspNetCore.Mvc;
using API_project_system.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using API_project_system.ModelsDto;

namespace API_project_system.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Authorize]
    public class AccountControler : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountControler(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult RegisterAccount([FromBody] RegisterUserDto registerUserDto)
        {
            string token = accountService.RegisterAccount(registerUserDto);
            return Ok(token);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login([FromBody] LoginUserDto loginUserDto)
        {
            string token = accountService.TryLoginUserAndGenerateJwt(loginUserDto);
            return Ok(token);
        }

        [HttpGet("token")]
        public async Task<ActionResult> IsTokenValidAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
            string token = accountService.GetJwtTokenIfValid(accessToken);
            return Ok(token);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> LogoutAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
            accountService.Logout(accessToken);
            return Ok();
        }
    }
}
