using API_project_system.Entities;

namespace API_project_system.Transactions.Submissions
{
    public class AddSubmissionTransaction : ITransaction
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly int userId;
        public readonly Submission SubmissionToAdd;

        public AddSubmissionTransaction(IUnitOfWork unitOfWork, int userId, Submission submissionToAdd)
        {
            this.unitOfWork = unitOfWork;
            this.userId = userId;
            SubmissionToAdd = submissionToAdd;
        }

        public void Execute()
        {
            SubmissionToAdd.UserId = userId;
            SubmissionToAdd.SubmissionDateTime = DateTime.Now;
            SubmissionToAdd.LastEdit = DateTime.Now;
            unitOfWork.Submissions.Add(SubmissionToAdd);
        }
    }
}
