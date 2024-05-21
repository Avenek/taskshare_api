using Microsoft.EntityFrameworkCore;
using API_project_system.Entities;
using System.Linq.Expressions;

namespace API_project_system.Specifications.CourseSpecifications
{
    public class CourseByIdWithUsersSpecification : Specification<Course>
    {
        private readonly int courseId;

        public CourseByIdWithUsersSpecification(int courseId)
        {
            this.courseId = courseId;
        }

        public override Expression<Func<Course, bool>> ToExpression()
        {
            return f => f.Id == courseId;
        }

        public override IQueryable<Course> IncludeEntities(IQueryable<Course> queryable)
        {
            return queryable.Include(f => f.EnrolledUsers).Include(f => f.PendingUsers);
        }
    }
}
