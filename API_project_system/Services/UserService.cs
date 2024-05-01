using AutoMapper;
using Microsoft.EntityFrameworkCore;
using API_project_system.DbContexts;
using API_project_system.Entities;
using API_project_system.ModelsDto;
using API_project_system.Repositories;
using API_project_system.Exceptions;
using API_project_system.Logger;
using API_project_system.Specifications;
using API_project_system.Transactions;
using Microsoft.AspNetCore.Identity;
using API_project_system.Transactions.UserTransactions;

namespace API_project_system.Services
{
    public interface IUserService
    {
        IUnitOfWork UnitOfWork { get; }
        PageResults<UserDto> GetAll(GetAllQuery queryParameters);
        UserDto GetById(int id);
        UserDto GetLoggedUser();
        void UpdateUser(int userId, UpdateUserDto updateUserDto);
        void DeleteUser(int userId);
    }
    public class UserService : IUserService
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IMapper mapper;
        private readonly UserActionLogger logger;
        private readonly IPaginationService queryParametersService;
        private readonly IUserContextService userContextService;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IMapper mapper,
            UserActionLogger logger, IPaginationService queryParametersService, IUserContextService userContextService)
        {
            UnitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
            this.mapper = mapper;
            this.logger = logger;
            this.queryParametersService = queryParametersService;
            this.userContextService = userContextService;
        }

        public PageResults<UserDto> GetAll(GetAllQuery queryParameters)
        {
            var spec = new UsersWithRoleSpecification(queryParameters.SearchPhrase);
            var usersQuery = UnitOfWork.Users.GetBySpecification(spec);
            var result = queryParametersService.PreparePaginationResults<UserDto, User>(queryParameters, usersQuery, mapper);

            return result;

        }
        public UserDto GetById(int id)
        {
            var user = UnitOfWork.Users.GetById(id);
            var userDto = mapper.Map<UserDto>(user);
            return userDto;
        }

        public UserDto GetLoggedUser()
        {
            var userId = userContextService.GetUserId;
            var spec = new UserByIdWithRoleSpecification(userId);
            var user = UnitOfWork.Users.GetBySpecification(spec).FirstOrDefault();
            var userDto = mapper.Map<UserDto>(user);
            return userDto;
        }

        public void UpdateUser(int userId, UpdateUserDto updateUserDto)
        {
            var adminId = userContextService.GetUserId;
            var updatedUser = mapper.Map<User>(updateUserDto);
            if (updateUserDto.Password != null)
            {
                var hashedPassword = passwordHasher.HashPassword(updatedUser, updateUserDto.Password);
                updatedUser.PasswordHash = hashedPassword;
            }
            UpdateUserTransaction updateUserTransaction = new(UnitOfWork.Users, userId, updatedUser);
            updateUserTransaction.Execute();
            UnitOfWork.Commit();
            logger.Log(EUserAction.UpdateUser, adminId, DateTime.UtcNow, userId);
        }
        public void DeleteUser(int userId)
        {
            var adminId = userContextService.GetUserId;
            var user = UnitOfWork.Users.GetById(userId);
            if (user == null)
            {
                throw new NotFoundException("That user doesn't exist.");
            }
            DeleteEntityTransaction<User> deleteUserTransaction = new(UnitOfWork.Users, userId);
            deleteUserTransaction.Execute();
            UnitOfWork.Commit();
            logger.Log(EUserAction.DeleteUser, adminId, DateTime.UtcNow, userId);
        }

    }
}
