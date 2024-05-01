using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_project_system.Database.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("users");
			builder.HasIndex(e => e.RoleId, "user_ibfk_1");

			builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();
			builder.Property(e => e.Email).HasColumnName("email").IsRequired();
			builder.Property(e => e.Name).HasColumnName("name").IsRequired();
			builder.Property(e => e.Lastname).HasColumnName("lastname").IsRequired();
			builder.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
			builder.Property(e => e.RoleId).HasColumnName("role_id");
			builder.Property(e => e.StatusId).HasColumnName("status_id");

			builder.HasOne(d => d.Role)
				.WithMany(r => r.Users)
				.HasForeignKey(d => d.RoleId)
				.HasConstraintName("user_ibfk_1");

			builder.HasOne(d => d.Status)
				.WithMany(r => r.Users)
				.HasForeignKey(d => d.StatusId)
				.HasConstraintName("user_ibfk_2");

			builder.HasMany(e => e.EnrolledCourses)
				.WithMany(e => e.EnrolledUsers)
				.UsingEntity("courses_enrolled_users");

			builder.HasMany(e => e.PendingCourses)
				.WithMany(e => e.PendingUsers)
				.UsingEntity("courses_pending_users");

			builder.HasMany(e => e.OwnedCourses)
				.WithOne(e => e.Owner)
				.HasForeignKey(e => e.UserId);

		}
	}
}
