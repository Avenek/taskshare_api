namespace API_project_system.Entities;

public class Submission : IHasUserId
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public virtual User User { get; set; }
	public int AssignmentId { get; set; }
	public virtual Assignment Assignment { get; set; }
	public DateTime SubmissionDateTime { get; set; }
	public string StudentComment { get; set; }
	public virtual ICollection<SubmissionFile> Files { get; set; }
}