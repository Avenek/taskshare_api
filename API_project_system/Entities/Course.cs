namespace API_project_system.Entities;

public class Course
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string IconPath { get; set; }
	public int OwnerId { get; set; }
	public virtual User Owner { get; set; }
	public virtual ICollection<User> EnrolledUsers { get; set; }
	public virtual ICollection<User> PendingUsers { get; set; }
	public virtual ICollection<Assignment> Assignments { get; set; }
}