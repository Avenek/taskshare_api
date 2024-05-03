namespace API_project_system.Entities;

public class Course : IHasUserId
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string IconPath { get; set; }
	public int UserId { get; set; }
	public int YearStart { get; set; }
	public virtual User Owner { get; set; }
	public virtual ICollection<User> EnrolledUsers { get; set; } = new List<User>();
	public virtual ICollection<User> PendingUsers { get; set; } = new List<User>();
    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}