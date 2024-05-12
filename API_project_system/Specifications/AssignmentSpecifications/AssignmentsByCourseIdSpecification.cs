using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_project_system.Specifications.AssigmentSpecifications
{
    public class AssignmentsByCourseIdSpecification : Specification<Assignment>
    {
        private readonly int courseId;
        private readonly int userId;

        public AssignmentsByCourseIdSpecification(int courseId, int userId)
        {
            this.courseId = courseId;
            this.userId = userId;
        }

        public override Expression<Func<Assignment, bool>> ToExpression()
        {
            return f => f.CourseId == courseId && (f.Visibility || f.Course.UserId == userId);
        }

        public override IQueryable<Assignment> IncludeEntities(IQueryable<Assignment> queryable)
        {
            return queryable
                .Include(f => f.Course)
                    .ThenInclude(f => f.Owner)
                .Include(f => f.Course)
                    .ThenInclude(f => f.EnrolledUsers);
        }
    }

}