using API_project_system.Entities;
using API_project_system.Enums;

namespace API_project_system.ModelsDto.CourseDto
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IconPath { get; set; }
        public int YearStart { get; set; }
        public EStatus ApprovalStatus { get; set; }
        public UserDto Owner { get; set; }
    }
}
