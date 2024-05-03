using API_project_system.Entities;

namespace API_project_system.Transactions.Submissions
{
    public class UpdateSubmissionTransaction : ITransaction
    {
        private readonly Submission submissionToUpdate;
        private readonly string newStudentComment;

        public UpdateSubmissionTransaction(Submission submissionToUpdate, string newStudentComment)
        {
            this.submissionToUpdate = submissionToUpdate;
            this.newStudentComment = newStudentComment;
        }
        public void Execute()
        {
            submissionToUpdate.StudentComment = newStudentComment;
            submissionToUpdate.LastEdit = DateTime.UtcNow;
        }
    }
}
