using API_project_system.Entities;
using API_project_system.Repositories;

namespace API_project_system.Transactions
{
    public class DeleteEntityTransaction<TEntity> : ITransaction where TEntity : class
    {
        private readonly IRepository<TEntity> repository;
        private readonly int id;

        public DeleteEntityTransaction(IRepository<TEntity> repository, int id)
        {
            this.repository = repository;
            this.id = id;
        }

        public void Execute()
        {
            repository.Remove(id);
        }
    }
}
