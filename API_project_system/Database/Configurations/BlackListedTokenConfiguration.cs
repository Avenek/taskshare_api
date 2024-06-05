using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_project_system.Database.Configurations
{
    public class BlackListedTokenConfiguration
    {
        public void Configure(EntityTypeBuilder<BlackListedToken> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");
            builder.HasIndex(e => e.UserId, "black_listed_tokens_ibfk_1");

            builder.ToTable("black_listed_tokens");

            builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();
            builder.Property(e => e.Token).HasColumnName("token").IsRequired();

            builder.HasOne(d => d.User)
              .WithMany(r => r.BlackListedTokens)
              .HasForeignKey(d => d.UserId)
              .HasConstraintName("black_listed_tokens_ibfk_1");

        }
    }
}
