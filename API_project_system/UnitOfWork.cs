using API_project_system.DbContexts;
using API_project_system.Entities;
using API_project_system.Repositories;


public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<Role> Roles { get; }
    IRepository<ApprovalStatus> ApprovalStatuses { get; }
    void Commit();
}
public class UnitOfWork : IUnitOfWork
{

    private readonly SystemDbContext dbContext;
    private Repository<User> users;
    private Repository<Role> roles;
    private Repository<ApprovalStatus> approvalStatuses;

    public UnitOfWork(SystemDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IRepository<User> Users
    {
        get
        {
            return users ??
                (users = new Repository<User>(dbContext));
        }
    }

    public IRepository<Role> Roles
    {
        get
        {
            return roles ??
                (roles = new Repository<Role>(dbContext));
        }
    }

    public IRepository<ApprovalStatus> ApprovalStatuses
    {
        get
        {
            return approvalStatuses ??
                (approvalStatuses = new Repository<ApprovalStatus>(dbContext));
        }
    }

    public void Commit()
    {
        dbContext.SaveChanges();
    }
}