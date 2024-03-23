using Microsoft.EntityFrameworkCore;
using API_project_system.DbContexts;

namespace API_project_system.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly SystemDbContext dbContext;

        public Repository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public DbSet<TEntity> Entity => dbContext.Set<TEntity>();

        public void Add(TEntity entity)
        {
            Entity.Add(entity);
            dbContext.SaveChanges();
        }

        public void Remove(int id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }

        }

        public TEntity GetById(int id)
        {
            return Entity.Find(id);
        }

        public List<TEntity> GetAll()
        {
            return Entity.ToList();
        }
    }
}
