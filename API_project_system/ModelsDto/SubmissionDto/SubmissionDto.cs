using API_project_system.Entities;

namespace API_project_system.ModelsDto.SubmissionDto
{
    public class SubmissionDto
    {
        public DateTime SubmissionDateTime { get; set; }
        public DateTime LastEdit { get; set; }
        public string StudentComment { get; set; }
        public virtual ICollection<SubmissionFile> Files { get; set; } = new List<SubmissionFile>();
    }
}
