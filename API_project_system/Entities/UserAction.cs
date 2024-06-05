namespace API_project_system.Entities
{
    public class UserAction
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserLog> Logs { get; set; } = new List<UserLog>();
    }
}
