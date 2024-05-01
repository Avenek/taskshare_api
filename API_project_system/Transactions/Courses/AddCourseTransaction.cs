using API_project_system.Exceptions;
using API_project_system.Entities;

namespace API_project_system.Transactions.Courses
{
    public class AddCourseTransaction : ITransaction
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly int userId;
        public readonly Course CourseToAdd;

        public AddCourseTransaction(IUnitOfWork unitOfWork, int userId, Course courseToAdd)
        {
            this.unitOfWork = unitOfWork;
            this.userId = userId;
            this.CourseToAdd = courseToAdd;
        }

        public void Execute()
        {
            CourseToAdd.UserId = userId;
            unitOfWork.Courses.Add(CourseToAdd);
        }
    }
}
