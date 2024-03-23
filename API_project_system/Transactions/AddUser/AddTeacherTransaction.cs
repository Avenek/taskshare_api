using API_project_system.Entities;
using API_project_system.Enums;
using API_project_system.Repositories;

namespace API_project_system.Transactions.AddUser
{
    public class AddTeacherTransaction : AddUserTransaction
    {
        public AddTeacherTransaction(IRepository<User> userRepository, User userToAdd) : base(userRepository, userToAdd)
        {
        }

        public override int MakeApprovalStatus()
        {
            return (int)ApprovalStatuses.NeedsConfirmation;
        }
    }
}
