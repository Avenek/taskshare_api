﻿using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_project_system.Database.Configurations
{
    public class CoursePendingUserConfiguration : IEntityTypeConfiguration<CoursePendingUser>
    {
        public void Configure(EntityTypeBuilder<CoursePendingUser> builder)
        {
            builder.HasIndex(e => e.UserId, "course_pending_users_ibfk_1");
            builder.HasIndex(e => e.CourseId, "course_pending_users_ibfk_2");
            builder.HasIndex(e => new { e.UserId, e.CourseId }).IsUnique();


            builder.ToTable("course_pending_users");

            builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

            builder.Property(e => e.CourseId).HasColumnName("course_id").IsRequired();


            builder.HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(user => user.UserId)
                .HasConstraintName("course_pending_users_ibfk_1");

            builder.HasOne(f => f.Course)
                .WithMany()
                .HasForeignKey(f => f.CourseId)
                .HasConstraintName("course_pending_users_ibfk_2");


        }
    }
}
