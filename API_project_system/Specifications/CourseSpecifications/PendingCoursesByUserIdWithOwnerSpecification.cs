using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace API_project_system.Specifications.CourseSpecifications
{
    public class PendingCoursesByUserIdWithOwnerSpecification : Specification<CoursePendingUser>
    {
        private readonly string? searchPhrase;
        private readonly int userId;

        public PendingCoursesByUserIdWithOwnerSpecification(int userId, string? searchPhrase)
        {
            this.searchPhrase = searchPhrase;
            this.userId = userId;
        }

        public override Expression<Func<CoursePendingUser, bool>> ToExpression()
        {
            return f => f.UserId == userId && (searchPhrase == null || f.Course.Name.ToLower().Contains(searchPhrase.ToLower()));
        }

        public override IQueryable<CoursePendingUser> IncludeEntities(IQueryable<CoursePendingUser> queryable)
        {
            return queryable.Include(f => f.Course).ThenInclude(f => f.Owner);
        }
    }
}
