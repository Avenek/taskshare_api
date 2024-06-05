using API_project_system.Specifications;
using Microsoft.EntityFrameworkCore;

namespace API_project_system.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        DbSet<TEntity> Entity { get; }
        void Add(TEntity entity);
        void Remove(int id);
        TEntity GetById(int id);
        List<TEntity> GetAll();
        List<TEntity> GetAllByUser(int userId);
        IQueryable<TEntity> GetBySpecification(Specification<TEntity> spec);
    }
}
