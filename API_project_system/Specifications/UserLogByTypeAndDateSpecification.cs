using Microsoft.EntityFrameworkCore;
using API_project_system.Entities;
using API_project_system.Logger;
using System.Linq.Expressions;

namespace API_project_system.Specifications
{
    public class UserLogByTypeAndDateSpecification : Specification<UserLog>
    {
        private readonly EUserAction type;
        private readonly DateTime start;
        private readonly DateTime end;
        private readonly int userId;

        public UserLogByTypeAndDateSpecification(EUserAction type, DateTime start, DateTime end, int userId)
        {
            this.type = type;
            this.start = start;
            this.end = end;
            this.userId = userId;
        }

        public override Expression<Func<UserLog, bool>> ToExpression()
        {
            return f => (type == 0 || f.ActionId == (int)type) && f.LogTime > start && f.LogTime < end && (userId == 0 || f.UserId == userId);
        }
    }
}
