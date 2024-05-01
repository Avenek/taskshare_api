using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_project_system.Specifications.CourseSpecifications
{
    public class EnrolledCoursesByUserIdWithOwnerSpecification : Specification<CourseEnrolledUser>
    {
        private readonly string? searchPhrase;
        private readonly int userId;

        public EnrolledCoursesByUserIdWithOwnerSpecification(int userId, string? searchPhrase)
        {
            this.searchPhrase = searchPhrase;
            this.userId = userId;
        }

        public override Expression<Func<CourseEnrolledUser, bool>> ToExpression()
        {
            return f => f.UserId == userId && (searchPhrase == null || f.Course.Name.Contains(searchPhrase, StringComparison.CurrentCultureIgnoreCase));
        }

        public override IQueryable<CourseEnrolledUser> IncludeEntities(IQueryable<CourseEnrolledUser> queryable)
        {
            return queryable.Include(f => f.Course).ThenInclude(f => f.Owner);
        }
    }
}
