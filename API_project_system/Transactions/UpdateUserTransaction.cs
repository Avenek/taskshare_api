using API_project_system.Entities;
using API_project_system.ModelsDto;
using API_project_system.Repositories;

namespace API_project_system.Transactions
{
    public class UpdateUserTransaction : ITransaction
    {
        private readonly IRepository<User> userRepository;
        private readonly int userId;
        private readonly User updatedUser;

        public UpdateUserTransaction(IRepository<User> userRepository, int userId, User updatedUser)
        {
            this.userRepository = userRepository;
            this.userId = userId;
            this.updatedUser = updatedUser;
        }
        public void Execute()
        {
            User userToUpdate = userRepository.GetById(userId);
            userToUpdate.Email = updatedUser.Email;
            userToUpdate.Nickname = updatedUser.Nickname;
            userToUpdate.PasswordHash = updatedUser.PasswordHash;
            userToUpdate.RoleId = updatedUser.RoleId;

        }
    }
}
