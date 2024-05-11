namespace API_project_system.ModelsDto.AssignmentDto
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Visibility { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string Description { get; set; }
    }
}