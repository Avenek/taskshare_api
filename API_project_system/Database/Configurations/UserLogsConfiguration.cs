using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using API_project_system.Entities;

namespace API_project_system.Database.Configurations
{
    public class UserLogsConfiguration : IEntityTypeConfiguration<UserLog>
    {
        public void Configure(EntityTypeBuilder<UserLog> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("user_logs");
            builder.HasIndex(e => e.UserId, "user_logs_ibfk_1");
            builder.HasIndex(e => e.ActionId, "user_logs_ibfk_2");

            builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            builder.Property(e => e.ActionId).HasColumnName("action_id").IsRequired();
            builder.Property(e => e.ObjectId).HasColumnName("object_id").HasDefaultValueSql(null);
            builder.Property(e => e.LogTime).HasColumnName("log_time");

            builder.HasOne(r => r.User)
                .WithMany(u => u.Logs)
                .HasForeignKey(user => user.UserId)
                .HasConstraintName("user_logs_ibfk_1");

            builder.HasOne(r => r.Action)
                .WithMany(u => u.Logs)
                .HasForeignKey(user => user.ActionId)
                .HasConstraintName("user_logs_ibfk_2");

        }
    }
}
