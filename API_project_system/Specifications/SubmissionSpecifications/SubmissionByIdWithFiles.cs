using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_project_system.Specifications.SubmissionSpecifications
{
    public class SubmissionByIdWithFiles : Specification<Submission>
    {
        private readonly int submissionId;

        public SubmissionByIdWithFiles(int submissionId)
        {
            this.submissionId = submissionId;
        }

        public override Expression<Func<Submission, bool>> ToExpression()
        {
            return f => f.Id == submissionId;
        }

        public override IQueryable<Submission> IncludeEntities(IQueryable<Submission> queryable)
        {
            return queryable.Include(f => f.Files);
        }
    }
}
