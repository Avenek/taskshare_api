using API_project_system.Entities;

namespace API_project_system.Entities
{
    public class BlackListedToken : IHasUserId
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
