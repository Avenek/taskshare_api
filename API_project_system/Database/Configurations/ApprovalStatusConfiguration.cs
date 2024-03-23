using API_project_system.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace API_project_system.Database.Configurations
{
    public class ApprovalStatusConfiguration : IEntityTypeConfiguration<ApprovalStatus>
    {
        public void Configure(EntityTypeBuilder<ApprovalStatus> builder)
        {
            builder.ToTable("approval_statuses");

            builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasColumnName("name").IsRequired();
            builder.HasMany(r => r.Users)
                .WithOne(u => u.Status)
                .HasForeignKey(user => user.StatusId)
                .HasConstraintName("user_ibfk_2");

        }
    }
}
