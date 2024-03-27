using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace API_project_system.Database.Configurations;
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
	public void Configure(EntityTypeBuilder<Course> builder)
	{
		builder.ToTable("courses");

		builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();
		builder.Property(e => e.Name).HasColumnName("name").IsRequired();
		builder.Property(e => e.IconPath).HasColumnName("icon_path");
		builder.Property(e => e.OwnerId).HasColumnName("owner_id").IsRequired();

		builder.HasMany(e => e.Assignments)
			.WithOne(e => e.Course)
			.HasForeignKey(e => e.CourseId);

		builder.HasMany(e => e.EnrolledUsers)
			.WithMany(e => e.EnrolledCourses)
			.UsingEntity("courses_enrolled_users");

		builder.HasMany(e => e.PendingUsers)
			.WithMany(e => e.PendingCourses)
			.UsingEntity("courses_pending_users");

		builder.HasOne(a => a.Owner)
		   .WithMany(c => c.OwnedCourses)
		   .HasForeignKey(a => a.OwnerId);
	}
}