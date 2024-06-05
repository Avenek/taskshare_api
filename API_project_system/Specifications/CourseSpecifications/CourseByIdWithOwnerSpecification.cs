using API_project_system.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;

namespace API_project_system.Specifications.CourseSpecifications
{
    public class CourseByIdWithOwnerSpecification : Specification<Course>
    {
        private readonly int id;

        public CourseByIdWithOwnerSpecification(int id)
        {

            this.id = id;
        }

        public override Expression<Func<Course, bool>> ToExpression()
        {
            return f => f.Id == id;
        }

        public override IQueryable<Course> IncludeEntities(IQueryable<Course> queryable)
        {
            return queryable.Include(f => f.Owner);
        }
    }
}
