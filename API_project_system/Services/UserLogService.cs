using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API_project_system.Entities;
using API_project_system.Exceptions;
using API_project_system.Logger;
using API_project_system.ModelsDto;
using API_project_system.Specifications;
using API_project_system.Transactions;
using System.IO.Compression;
using System.Text;
namespace API_project_system.Services
{

    public interface IUserLogService
    {
        public IUnitOfWork UnitOfWork { get; }
        IEnumerable<UserLog> Get(string type, long startTimestamp, long endTimestamp, int userId);
    }

    public class UserLogService : IUserLogService
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly IMapper mapper;

        public UserLogService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public IEnumerable<UserLog> Get(string type, long startTimestamp, long endTimestamp, int userId)
        {
            if(Enum.TryParse(typeof(EUserAction), type, true, out var actionType))
            {
                EUserAction action = (EUserAction)actionType;
                DateTime startDateTime = ConvertTimestampToDateTime(startTimestamp);
                DateTime endDateTime = ConvertTimestampToDateTime(endTimestamp);
                var spec = new UserLogByTypeAndDateSpecification(action, startDateTime, endDateTime, userId);
                var actions = UnitOfWork.UserLogs.GetBySpecification(spec).ToList();
                return actions;
            }
            else
            {
                throw new BadRequestException("Cannot parse userAction to enumType.");
            }
        }

        private DateTime ConvertTimestampToDateTime(long timestamp)
        {
            DateTimeOffset offset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            DateTime dateTime = offset.LocalDateTime;
            return dateTime;
        }
    }

}
