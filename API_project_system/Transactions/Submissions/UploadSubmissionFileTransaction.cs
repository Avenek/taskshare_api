using API_project_system.Entities;

namespace API_project_system.Transactions.Submissions
{
    public class UploadSubmissionFileTransaction : ITransaction
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly int submissionId;
        private readonly string path;

        public UploadSubmissionFileTransaction(IUnitOfWork unitOfWork, int submissionId, string path)
        {
            this.unitOfWork = unitOfWork;
            this.submissionId = submissionId;
            this.path = path;
        }

        public void Execute()
        {
           SubmissionFile submissionFile = new SubmissionFile() { SubmissionId = submissionId, FilePath = path };
            unitOfWork.SubmissionFiles.Add(submissionFile);
        }
    }
}
