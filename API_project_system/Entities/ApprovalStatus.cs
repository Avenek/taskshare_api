namespace API_project_system.Entities
{
    public class ApprovalStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}
