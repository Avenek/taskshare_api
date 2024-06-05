using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace API_project_system.Database.Configurations;
public class SubmissionFileConfiguration : IEntityTypeConfiguration<SubmissionFile>
{
	public void Configure(EntityTypeBuilder<SubmissionFile> builder)
	{
		builder.ToTable("submission_files");

		builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();
		builder.Property(e => e.SubmissionId).HasColumnName("submission_id").IsRequired();
		builder.Property(e => e.FilePath).HasColumnName("file_path").IsRequired();

		builder.HasOne(a => a.Submission)
		   .WithMany(c => c.Files)
		   .HasForeignKey(a => a.SubmissionId)
		   .OnDelete(DeleteBehavior.Cascade);
	}
}