using API_project_system.Entities;
using API_project_system.ModelsDto;
using API_project_system.Repositories;

namespace API_project_system.Transactions.UserTransactions
{
    public class UpdateUserTransaction : ITransaction
    {
        private readonly IRepository<User> repository;
        private readonly int userId;
        private readonly User updatedUser;

        public UpdateUserTransaction(IRepository<User> repository, int userId, User updatedUser)
        {
            this.repository = repository;
            this.userId = userId;
            this.updatedUser = updatedUser;
        }
        public void Execute()
        {
            User userToUpdate = repository.GetById(userId);
            userToUpdate.Email = updatedUser.Email;
            userToUpdate.Name = updatedUser.Name;
            userToUpdate.Lastname = updatedUser.Lastname;
            userToUpdate.RoleId = updatedUser.RoleId;
            if(updatedUser.PasswordHash != null)
            {
                userToUpdate.PasswordHash = updatedUser.PasswordHash;
            }

        }
    }
}
