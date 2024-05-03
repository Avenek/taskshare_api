namespace API_project_system.Entities;

public class Assignment
{
	public int Id { get; set; }
	public string Name { get; set; }
	public DateTime DeadlineDate { get; set; }
	public bool Visibility { get; set; }
	public string Description { get; set; }
	public int CourseId { get; set; }
	public virtual Course Course { get; set; }
	public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}