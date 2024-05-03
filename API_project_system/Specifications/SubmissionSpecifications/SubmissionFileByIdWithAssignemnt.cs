using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_project_system.Specifications.SubmissionSpecifications
{
    public class SubmissionFileByIdWithAssignemnt : Specification<SubmissionFile>
    {
        private readonly int fileId;

        public SubmissionFileByIdWithAssignemnt(int fileId)
        {
            this.fileId = fileId;
        }

        public override Expression<Func<SubmissionFile, bool>> ToExpression()
        {
            return f => f.Id == fileId;
        }

        public override IQueryable<SubmissionFile> IncludeEntities(IQueryable<SubmissionFile> queryable)
        {
            return queryable
                .Include(f => f.Submission)
                    .ThenInclude(f => f.Assignment)
                    .ThenInclude(f => f.Course)
                .Include(f => f.Submission)
                    .ThenInclude(f => f.Assignment)
                    .ThenInclude(f => f.Course)
                    .ThenInclude(f => f.EnrolledUsers);
        }
    }
}
