using API_project_system.DbContexts;
using API_project_system.Entities;
using API_project_system.Repositories;

namespace API_project_system
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<BlackListedToken> BlackListedTokens { get; }
        IRepository<Role> Roles { get; }
        IRepository<ApprovalStatus> ApprovalStatuses { get; }
        IRepository<Assignment> Assignments { get; }
        IRepository<Course> Courses { get; }
        IRepository<CourseEnrolledUser> CoursesEnrolledUsers { get; }
        IRepository<CoursePendingUser> CoursesPendingUsers { get; }
        IRepository<Submission> Submissions { get; }
        IRepository<SubmissionFile> SubmissionFiles { get; }
        IRepository<UserAction> UserActions { get; }
        IRepository<UserLog> UserLogs { get; }
        void Commit();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SystemDbContext dbContext;

        private IRepository<User> users;
        private Repository<BlackListedToken> blackListedTokens;
        private IRepository<Role> roles;
        private IRepository<ApprovalStatus> approvalStatuses;
        private IRepository<Assignment> assignments;
        private IRepository<Course> courses;
        private IRepository<CourseEnrolledUser> coursesEnrolledUsers;
        private IRepository<CoursePendingUser> coursesPendingUsers;
        private IRepository<Submission> submissions;
        private IRepository<SubmissionFile> submissionFiles;
        private Repository<UserAction> userActions;
        private Repository<UserLog> userLogs;

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

        public IRepository<BlackListedToken> BlackListedTokens
        {
            get
            {
                return blackListedTokens ??
                    (blackListedTokens = new Repository<BlackListedToken>(dbContext));
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
                    (assignments = new Repository<Assignment>(dbContext));
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
        public IRepository<CourseEnrolledUser> CoursesEnrolledUsers
        {
            get
            {
                return coursesEnrolledUsers ??
                    (coursesEnrolledUsers = new Repository<CourseEnrolledUser>(dbContext));
            }
        }

        public IRepository<CoursePendingUser> CoursesPendingUsers
        {
            get
            {
                return coursesPendingUsers ??
                    (coursesPendingUsers = new Repository<CoursePendingUser>(dbContext));
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

        public IRepository<UserAction> UserActions
        {
            get
            {
                return userActions ??
                    (userActions = new Repository<UserAction>(dbContext));
            }
        }

        public IRepository<UserLog> UserLogs
        {
            get
            {
                return userLogs ??
                    (userLogs = new Repository<UserLog>(dbContext));
            }
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }
    }
}