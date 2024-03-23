using API_project_system.Entities;
using API_project_system.Repositories;
using System.Transactions;

namespace API_project_system.Transactions
{
    public class AddUserTransaction : ITransaction
    {
        private readonly IRepository<User> userRepository;
        private readonly User userToAdd;

        public AddUserTransaction(IRepository<User> userRepository, User userToAdd)
        {
            this.userRepository = userRepository;
            this.userToAdd = userToAdd;
        }

        public void Execute()
        {
            userRepository.Add(userToAdd);
        }
    }
}
