using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using API_project_system.Entities;
using API_project_system.Exceptions;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;

namespace API_project_system.DbContexts
{
    public class SystemDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }

        public SystemDbContext(DbContextOptions<SystemDbContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly()
            );
        }

    }

}
