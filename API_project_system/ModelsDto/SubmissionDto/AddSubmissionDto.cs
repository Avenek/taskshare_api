using API_project_system.Entities;

namespace API_project_system.ModelsDto.SubmissionDto
{
    public class AddSubmissionDto
    {
        public int AssignmentId { get; set; }
        public string StudentComment { get; set; }
    }
}
