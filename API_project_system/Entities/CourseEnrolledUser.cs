namespace API_project_system.Entities
{
    public class CourseEnrolledUser
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }

        public virtual User User { get; set; }
        public virtual Course Course { get; set; }
    }
}
