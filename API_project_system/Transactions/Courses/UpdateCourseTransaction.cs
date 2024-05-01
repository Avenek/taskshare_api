using API_project_system.Entities;

namespace API_project_system.Transactions.Courses
{
    public class UpdateCourseTransaction : ITransaction
    {
        private readonly Course courseToUpdate;
        private readonly string? newName;
        private readonly string? newIconPath;

        public UpdateCourseTransaction(Course courseToUpdate, string? newName, string? newIconPath)
        {
            this.courseToUpdate = courseToUpdate;
            this.newName = newName;
            this.newIconPath = newIconPath;
        }
        public void Execute()
        {
            if (newName is not null)
            {
                courseToUpdate.Name = newName;
            }
            if (newIconPath is not null)
            {
                courseToUpdate.IconPath = newIconPath;
            }
        }
    }
}
