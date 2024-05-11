//add necessary libraries
using API_project_system.Entities;

namespace API_project_system.Transactions.Assignments
{
    public class AddAssignmentTransaction : ITransaction
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly Assignment assignmentToAdd;

        public AddAssignmentTransaction(IUnitOfWork unitOfWork, Assignment assignmentToAdd)
        {
            this.unitOfWork = unitOfWork;
            this.assignmentToAdd = assignmentToAdd;
        }

        public void Execute()
        {
            unitOfWork.Assignments.Add(assignmentToAdd);
        }
    }
}
