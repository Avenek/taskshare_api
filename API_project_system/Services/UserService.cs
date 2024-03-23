using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API_project_system.DbContexts;
using API_project_system.Entities;
using API_project_system.ModelsDto;
using API_project_system.Repositories;

namespace API_project_system.Services
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();

    }
    public class UserService : IUserService
    {
        private readonly IRepository<User> userRepository;

        public UserService(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }

        public IEnumerable<UserDto> GetAll()
        {
            return null;
        }
    }
}
