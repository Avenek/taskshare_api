using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;

namespace API_project_system.Specifications.CourseSpecifications
{
    public class OwnedCoursesByUserIdWithOwnerSpecification : Specification<Course>
    {
        private readonly string? searchPhrase;
        private readonly int userId;

        public OwnedCoursesByUserIdWithOwnerSpecification(int userId, string? searchPhrase)
        {
            this.searchPhrase = searchPhrase;
            this.userId = userId;
        }

        public override Expression<Func<Course, bool>> ToExpression()
        {
            return f => f.UserId == userId && (searchPhrase == null || f.Name.ToLower().Contains(searchPhrase.ToLower()));
        }

        public override IQueryable<Course> IncludeEntities(IQueryable<Course> queryable)
        {
            return queryable.Include(f => f.Owner);
        }
    }
}
