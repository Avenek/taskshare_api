using System.Linq.Expressions;

namespace API_project_system.Specifications
{
    public abstract class Specification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public virtual IQueryable<T> IncludeEntities(IQueryable<T> queryable)
        {
            return queryable;
        }
    }
}
