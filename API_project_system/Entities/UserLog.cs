namespace API_project_system.Entities
{
    public class UserLog : IHasUserId
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ActionId { get; set; }
        public int? ObjectId { get; set; } = null;
        public DateTime LogTime { get; set; }

        public virtual User User { get; set; }
        public virtual UserAction Action { get; set; }

    }
}
