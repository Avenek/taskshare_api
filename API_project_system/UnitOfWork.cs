using API_project_system.DbContexts;
using API_project_system.Entities;
using API_project_system.Repositories;


public interface IUnitOfWork
{
    IRepository<User> Users { get; }
    IRepository<Role> Roles { get; }
    IRepository<ApprovalStatus> ApprovalStatuses { get; }
    IRepository<Assignment> Assignments { get; }
    IRepository<Course> Courses { get; }
    IRepository<Submission> Submissions { get; }
    IRepository<SubmissionFile> SubmissionFiles { get; }
    void Commit();
}
public class UnitOfWork : IUnitOfWork
{

    private readonly SystemDbContext dbContext;
    private IRepository<User> users;
    private IRepository<Role> roles;
    private IRepository<ApprovalStatus> approvalStatuses;
    private IRepository<Assignment> assignments;
    private IRepository<Course> courses;
    private IRepository<Submission> submissions;
    private IRepository<SubmissionFile> submissionFiles;

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

    public IRepository<Assignment> Assignments
    {
        get
        {
            return assignments ??
                (assignments= new Repository<Assignment>(dbContext));
        }
    }

    public IRepository<Course> Courses
    {
        get
        {
            return courses ??
                (courses = new Repository<Course>(dbContext));
        }
    }

    public IRepository<Submission> Submissions
    {
        get
        {
            return submissions ??
                (submissions = new Repository<Submission>(dbContext));
        }
    }

    public IRepository<SubmissionFile> SubmissionFiles
    {
        get
        {
            return submissionFiles ??
                (submissionFiles = new Repository<SubmissionFile>(dbContext));
        }
    }

    public void Commit()
    {
        dbContext.SaveChanges();
    }
}