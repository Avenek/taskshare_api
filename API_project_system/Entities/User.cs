using Microsoft.AspNetCore.Identity;

namespace API_project_system.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public int StatusId { get; set; }

        public virtual ApprovalStatus Status { get; set; }

        public virtual Role Role { get; set; }

    }
}
