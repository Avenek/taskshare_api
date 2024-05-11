namespace API_project_system.Entities;

public class Assignment : IHasUserId
{
    public int Id { get; set; }
    public virtual int UserId { get { return Course.UserId; } }
    public string Name { get; set; }
    public DateTime DeadlineDate { get; set; }
    public bool Visibility { get; set; }
    public string Description { get; set; }
    public int CourseId { get; set; }
    public virtual Course Course { get; set; }
    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}