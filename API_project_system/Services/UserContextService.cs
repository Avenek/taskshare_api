using System.Security.Claims;

namespace API_project_system.Services
{
    public interface IUserContextService
    {
        int GetUserId { get; }
        ClaimsPrincipal User { get; set; }
    }

    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public int GetUserId => int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

        public ClaimsPrincipal User
        {
            get => httpContextAccessor.HttpContext?.User;
            set => User = value;
        }
    }
}
