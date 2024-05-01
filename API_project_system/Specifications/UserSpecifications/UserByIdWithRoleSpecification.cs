using Microsoft.EntityFrameworkCore;
using API_project_system.Entities;
using System.Linq.Expressions;

namespace API_project_system.Specifications
{
    public class UserByIdWithRoleSpecification : Specification<User>
    {
        private readonly int id;

        public UserByIdWithRoleSpecification(int id)
        {
            this.id = id;
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            return f => f.Id == id;
        }

        public override IQueryable<User> IncludeEntities(IQueryable<User> queryable)
        {
            return queryable.Include(f => f.Role);
        }
    }
}
