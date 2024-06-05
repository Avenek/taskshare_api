using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_project_system.Specifications.SubmissionFileSpecifications
{
    public class SubmissionFileByUserIdOrOwnerId : Specification<SubmissionFile>
    {
        private readonly int userId;
        private readonly int courseOwnerId;

        public SubmissionFileByUserIdOrOwnerId(int userId, int courseOwnerId)
        {
            this.userId = userId;
            this.courseOwnerId = courseOwnerId;
        }

        public override Expression<Func<SubmissionFile, bool>> ToExpression()
        {
            return f => f.Submission.Assignment.UserId == courseOwnerId && f.Submission.UserId == userId;
        }

        public override IQueryable<SubmissionFile> IncludeEntities(IQueryable<SubmissionFile> queryable)
        {
            return queryable.Include(f => f.Submission).ThenInclude(f => f.Assignment);
        }
    }
}
