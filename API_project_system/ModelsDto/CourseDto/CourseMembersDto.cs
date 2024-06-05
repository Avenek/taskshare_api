namespace API_project_system.ModelsDto
{
    public class CourseMembersDto
    {
        public int Id { get; set; }
        public virtual ICollection<UserDto> EnrolledUsers { get; set; } = new List<UserDto>();
        public virtual ICollection<UserDto> PendingUsers { get; set; } = new List<UserDto>();
    }
}
