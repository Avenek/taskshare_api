using API_project_system.Entities;
using API_project_system.ModelsDto.SubmissionFileDtos;

namespace API_project_system.ModelsDto.SubmissionDto
{
    public class SubmissionDto
    {
        public int Id { get; set; }
        public UserDto User { get; set; }
        public DateTime SubmissionDateTime { get; set; }
        public DateTime LastEdit { get; set; }
        public string StudentComment { get; set; }
        public virtual ICollection<SubmissionFileDto> Files { get; set; } = new List<SubmissionFileDto>();
    }
}
