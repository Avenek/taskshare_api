using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_project_system.Specifications.SubmissionSpecifications
{
    public class GetSubmissionByUserAndAssignmentId : Specification<Submission>
    {
        private readonly int assignmentId;
        private readonly int userId;

        public GetSubmissionByUserAndAssignmentId(int assignmentId, int userId)
        {
            this.assignmentId = assignmentId;
            this.userId = userId;
        }

        public override Expression<Func<Submission, bool>> ToExpression()
        {
            return f => f.AssignmentId == assignmentId && f.UserId == userId;
        }

        public override IQueryable<Submission> IncludeEntities(IQueryable<Submission> queryable)
        {
            return queryable.Include(f => f.Files);
        }
    }
}
