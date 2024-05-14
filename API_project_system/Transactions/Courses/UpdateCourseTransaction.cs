using API_project_system.Entities;

namespace API_project_system.Transactions.Courses
{
    public class UpdateCourseTransaction : ITransaction
    {
        private readonly Course courseToUpdate;
        private readonly string? newName;
        private readonly string? newIconPath;
        private readonly int? newYearStart;

        public UpdateCourseTransaction(Course courseToUpdate, string? newName, string? newIconPath, int? newYearStart)
        {
            this.courseToUpdate = courseToUpdate;
            this.newName = newName;
            this.newIconPath = newIconPath;
            this.newYearStart = newYearStart;
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
            if(newYearStart is not null)
            {
                courseToUpdate.YearStart = newYearStart.Value;
            }
        }
    }
}
