using API_project_system.Entities;

namespace API_project_system.Transactions.Courses
{
    public class AcceptMemberTransaction : ITransaction
    {
        private readonly Course courseToUpdate;
        private readonly User userToAccept;

        public AcceptMemberTransaction(Course courseToUpdate, User userToAccept)
        {
            this.courseToUpdate = courseToUpdate;
            this.userToAccept = userToAccept;
        }
        public void Execute()
        {
            courseToUpdate.PendingUsers.Remove(userToAccept);
            courseToUpdate.EnrolledUsers.Add(userToAccept);
        }
    }
}
