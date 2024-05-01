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
		builder.Property(e => e.UserId).HasColumnName("owner_id").IsRequired();

		builder.HasMany(e => e.Assignments)
			.WithOne(e => e.Course)
			.HasForeignKey(e => e.CourseId);

        builder.HasMany(i => i.PendingUsers)
                              .WithMany(t => t.PendingCourses)
                              .UsingEntity<CoursePendingUser>(
                                  j => j.HasOne(pt => pt.User).WithMany().HasForeignKey(pt => pt.UserId),
                                  j => j.HasOne(pt => pt.Course).WithMany().HasForeignKey(pt => pt.CourseId),
                                  j =>
                                  {
                                      j.HasKey(pt => new { pt.UserId, pt.CourseId });
                                      j.ToTable("course_pending_users");
                                  });

        builder.HasMany(i => i.EnrolledUsers)
                              .WithMany(t => t.EnrolledCourses)
                              .UsingEntity<CourseEnrolledUser>(
                                  j => j.HasOne(pt => pt.User).WithMany().HasForeignKey(pt => pt.UserId),
                                  j => j.HasOne(pt => pt.Course).WithMany().HasForeignKey(pt => pt.CourseId),
                                  j =>
                                  {
                                      j.HasKey(pt => new { pt.UserId, pt.CourseId });
                                      j.ToTable("course_enrolled_users");
                                  });

        builder.HasOne(a => a.Owner)
		   .WithMany(c => c.OwnedCourses)
		   .HasForeignKey(a => a.UserId);
	}
}