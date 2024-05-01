using API_project_system.Entities;
using API_project_system.Repositories;

namespace API_project_system.Logger
{
    public class UserActionLogger
    {
        private readonly IUnitOfWork unitOfWork;

        public UserActionLogger(IUnitOfWork unitOfWork)
        { 
            this.unitOfWork = unitOfWork;
        }
        public void Log(EUserAction action, int userId, DateTime time, int? objectId = null)
        {
            var userLog = new UserLog() { ActionId = (int)action, UserId = userId, LogTime = time };
            unitOfWork.UserLogs.Add(userLog);
            unitOfWork.Commit();
        }
    }
}
