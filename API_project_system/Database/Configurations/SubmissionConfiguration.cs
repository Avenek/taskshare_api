using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace API_project_system.Database.Configurations;
public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
{
	public void Configure(EntityTypeBuilder<Submission> builder)
	{
		builder.ToTable("submissions");

		builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();
		builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
		builder.Property(e => e.SubmissionDateTime).HasColumnName("submission_date_time").IsRequired();
		builder.Property(e => e.StudentComment).HasColumnName("student_comment");

		builder.HasMany(e => e.Files)
			.WithOne(e => e.Submission)
			.HasForeignKey(e => e.SubmissionId);

		builder.HasOne(a => a.User)
		   .WithMany(c => c.Submissions)
		   .HasForeignKey(a => a.UserId);
	}
}