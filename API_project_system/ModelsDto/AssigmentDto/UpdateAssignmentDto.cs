namespace API_project_system.ModelsDto.AssignmentDto
{
    public class UpdateAssignmentDto
    {
        public string? Name { get; set; }
        public bool? Visibility { get; set; }
        public string? Description { get; set; }
        public DateTime? DeadlineDate { get; set; }
    }
}