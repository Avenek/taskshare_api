namespace API_project_system.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string Lastname { get; set; }
		public string PasswordHash { get; set; }

		public int RoleId { get; set; }
		public int StatusId { get; set; }

		public virtual ApprovalStatus Status { get; set; }

		public virtual Role Role { get; set; }

		public virtual ICollection<Course> OwnedCourses { get; set; }
        public virtual ICollection<BlackListedToken> BlackListedTokens { get; set; } = new List<BlackListedToken>();
        public virtual ICollection<UserLog> Logs { get; set; } = new List<UserLog>();
		public virtual ICollection<Course> EnrolledCourses { get; set; } = new List<Course>();
		public virtual ICollection<Course> PendingCourses { get; set; } = new List<Course>();


		public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();

    }
}
