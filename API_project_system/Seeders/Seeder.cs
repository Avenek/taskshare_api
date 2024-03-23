using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API_project_system.DbContexts;
using API_project_system.Entities;
using System.Security.Cryptography;

namespace API_project_system.Seeders
{
    public class Seeder
    {
        private readonly SystemDbContext dbContext;

        public Seeder(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Seed()
        {
            if (dbContext.Database.CanConnect())
            {
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if(pendingMigrations != null && pendingMigrations.Any())
                {
                    dbContext.Database.Migrate();
                }

                if(!dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    dbContext.Roles.AddRange(roles);
                    dbContext.SaveChanges();
                }
                if (!dbContext.ApprovalStatuses.Any())
                {
                    var statuses = GetStatuses();
                    dbContext.ApprovalStatuses.AddRange(statuses);
                    dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            List<Role> roles = [new Role() { Name = "Admin" }, new Role() { Name = "Teacher" }, new Role() { Name = "Student" }];
            return roles;
        }

        private IEnumerable<ApprovalStatus> GetStatuses()
        {
            List<ApprovalStatus> statuses = [new ApprovalStatus() { Name = "Confirmed" }, new ApprovalStatus() { Name = "Needs confirmation" }];
            return statuses;
        }
    }
}
