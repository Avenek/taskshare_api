namespace API_project_system.Entities;
public class SubmissionFile
{
	public int Id { get; set; }
	public int SubmissionId { get; set; }
	public string FilePath { get; set; }
    public virtual Submission Submission { get; set; }
}