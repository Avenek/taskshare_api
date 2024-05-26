using Microsoft.EntityFrameworkCore;
using API_project_system.DbContexts;
using API_project_system.Entities;
using API_project_system.Logger;

namespace API_project_system.Seeders
{
    public class Seeder
    {
        private readonly SystemDbContext dbContext;
        private readonly IUnitOfWork unitOfWork;

        public Seeder(SystemDbContext dbContext, IUnitOfWork unitOfWork)
        {
            this.dbContext = dbContext;
            this.unitOfWork = unitOfWork;
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
                    unitOfWork.Roles.Entity.AddRange(roles);
                    unitOfWork.Commit();
                }
                if (!dbContext.ApprovalStatuses.Any())
                {
                    var statuses = GetStatuses();
                    unitOfWork.ApprovalStatuses.Entity.AddRange(statuses);
                    unitOfWork.Commit();
                }
                if (!unitOfWork.UserActions.Entity.Any())
                {
                    EUserAction[] userActions = (EUserAction[])Enum.GetValues(typeof(EUserAction));

                    foreach (var action in userActions)
                    {
                        if ((int)action > 0) unitOfWork.UserActions.Add(new UserAction() { Id = (int)action, Name = action.ToString() });
                    }
                    unitOfWork.Commit();

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
            List<ApprovalStatus> statuses = [new ApprovalStatus() { Name = "Confirmed" }, new ApprovalStatus() { Name = "Needs confirmation" }, new ApprovalStatus() { Name = "Owner" }];
            return statuses;
        }
    }
}
