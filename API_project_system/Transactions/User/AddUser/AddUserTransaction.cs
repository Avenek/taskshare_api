using API_project_system.Entities;
using API_project_system.Repositories;
using System.Transactions;

namespace API_project_system.Transactions.AddUser
{
    public abstract class AddUserTransaction : ITransaction
    {
        private readonly IRepository<User> userRepository;
        public User UserToAdd;

        public AddUserTransaction(IRepository<User> userRepository, User userToAdd)
        {
            this.userRepository = userRepository;
            this.UserToAdd = userToAdd;
        }

        public void Execute()
        {
            UserToAdd.StatusId = MakeApprovalStatus();
            userRepository.Add(UserToAdd);
        }

        public abstract int MakeApprovalStatus();
    }
}
