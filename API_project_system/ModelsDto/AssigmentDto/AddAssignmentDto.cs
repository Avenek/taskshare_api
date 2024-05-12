namespace API_project_system.ModelsDto.AssignmentDto
{
    public class AddAssignmentDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Visibility { get; set; }
        public DateTime DeadlineDate { get; set; }
        public int CourseId { get; set; }
    }
}