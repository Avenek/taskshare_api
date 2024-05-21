using API_project_system.Entities;

namespace API_project_system.Transactions.Courses
{
    public class JoinCourseTransaction : ITransaction
    {
        private readonly Course courseToUpdate;
        private readonly User userToJoin;

        public JoinCourseTransaction(Course courseToUpdate, User userToJoin)
        {
            this.courseToUpdate = courseToUpdate;
            this.userToJoin = userToJoin;
        }
        public void Execute()
        {
            courseToUpdate.PendingUsers.Add(userToJoin);
        }
    }
}
