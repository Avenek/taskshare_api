using Microsoft.EntityFrameworkCore;
using API_project_system.Entities;
using System.Linq.Expressions;

namespace API_project_system.Specifications
{
    public class UsersWithRoleSpecification : Specification<User>
    {
        private readonly string? searchPhrase;

        public UsersWithRoleSpecification(string? searchPhrase)
        {
            this.searchPhrase = searchPhrase;
        }

        public override Expression<Func<User, bool>> ToExpression()
        {
            return f => (searchPhrase == null || f.Name.Contains(searchPhrase, StringComparison.CurrentCultureIgnoreCase));
        }

        public override IQueryable<User> IncludeEntities(IQueryable<User> queryable)
        {
            return queryable.Include(f => f.Role);
        }
    }
}
