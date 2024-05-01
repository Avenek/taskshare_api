using Microsoft.EntityFrameworkCore;
using API_project_system.Entities;
using System.Linq.Expressions;

namespace API_project_system.Specifications
{
    public class UserByEmailWithRoleSpecification : Specification<User>
    {
        private readonly string email;

        public UserByEmailWithRoleSpecification(string email)
        {
            this.email = email;
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            return f => f.Email == email;
        }

        public override IQueryable<User> IncludeEntities(IQueryable<User> queryable)
        {
            return queryable.Include(f => f.Role);
        }
    }
}
