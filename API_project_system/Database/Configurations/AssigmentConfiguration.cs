using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using API_project_system.Entities;


namespace API_project_system.Database.Configurations;
public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
	public void Configure(EntityTypeBuilder<Assignment> builder)
	{
		builder.ToTable("assigments");

		builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();
		builder.Property(e => e.Name).HasColumnName("name").IsRequired();
		builder.Property(e => e.DeadlineDate).HasColumnName("deadline_date").IsRequired();
		builder.Property(e => e.Visibility).HasColumnName("visibility").IsRequired();
		builder.Property(e => e.Description).HasColumnName("description");
		builder.Property(e => e.CourseId).HasColumnName("course_id").IsRequired();

		builder.HasMany(d => d.Submissions)
			.WithOne(a => a.Assignment)
			.HasForeignKey(d => d.AssignmentId);

		builder.HasOne(a => a.Course)
		   .WithMany(c => c.Assignments)
		   .HasForeignKey(a => a.CourseId);
	}
}