using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using API_project_system.Entities;

namespace API_project_system.Database.Configurations
{
    public class UserActionConfiguration : IEntityTypeConfiguration<UserAction>
    {
        public void Configure(EntityTypeBuilder<UserAction> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("user_actions");

            builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasColumnName("name").IsRequired();

            builder.HasMany(r => r.Logs)
                .WithOne(u => u.Action)
                .HasForeignKey(user => user.ActionId)
                .HasConstraintName("user_actions_ibfk_1");

        }
    }
}
