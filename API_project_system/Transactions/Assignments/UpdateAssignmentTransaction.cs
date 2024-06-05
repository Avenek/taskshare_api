//include namespace
using API_project_system.Entities;
using API_project_system.ModelsDto.AssignmentDto;
namespace API_project_system.Transactions.Assignments
{
    public class UpdateAssignmentTransaction : ITransaction
    {
        private readonly Assignment assignmentToUpdate;
        private readonly UpdateAssignmentDto updateDto;

        public UpdateAssignmentTransaction(Assignment assignmentToUpdate, UpdateAssignmentDto updateDto)
        {
            this.assignmentToUpdate = assignmentToUpdate;
            this.updateDto = updateDto;
        }

        public void Execute()
        {
            if (updateDto.Name != null)
            {
                assignmentToUpdate.Name = updateDto.Name;
            }

            if (updateDto.DeadlineDate != null)
            {
                assignmentToUpdate.DeadlineDate = (DateTime)updateDto.DeadlineDate;
            }

            if (updateDto.Visibility != null)
            {
                assignmentToUpdate.Visibility = updateDto.Visibility.Value;
            }

            if (updateDto.Description != null)
            {
                assignmentToUpdate.Description = updateDto.Description;
            }
        }
    }
}
