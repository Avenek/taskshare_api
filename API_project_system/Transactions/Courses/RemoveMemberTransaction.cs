using API_project_system.Entities;

namespace API_project_system.Transactions.Courses
{
    public class RemoveMemberTransaction : ITransaction
    {
        private readonly Course courseToUpdate;
        private readonly User userToRemove;

        public RemoveMemberTransaction(Course courseToUpdate, User userToRemove)
        {
            this.courseToUpdate = courseToUpdate;
            this.userToRemove = userToRemove;
        }
        public void Execute()
        {
            courseToUpdate.PendingUsers.Remove(userToRemove);
            courseToUpdate.EnrolledUsers.Remove(userToRemove);
        }
    }
}
