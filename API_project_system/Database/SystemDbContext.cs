using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace API_project_system.DbContexts
{
	public class SystemDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
		public DbSet<Assignment> Assignments { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Submission> Submissions { get; set; }
		public DbSet<SubmissionFile> SubmissionFiles { get; set; }
		public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
		{
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder
				.UseSnakeCaseNamingConvention();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.ApplyConfigurationsFromAssembly(
				Assembly.GetExecutingAssembly()
			);
		}

	}

}
