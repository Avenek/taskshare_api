using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using API_project_system.Entities;
using API_project_system.Exceptions;
using API_project_system.ModelsDto;
using API_project_system.Repositories;
using API_project_system.Transactions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API_project_system.Transactions.AddUser;

namespace API_project_system.Services
{
    public interface IAccountService
    {
        IUnitOfWork UnitOfWork { get; }
        void RegisterUser(RegisterUserDto registerUserDto);
        string TryLoginUserAndGenerateJwt(LoginUserDto loginUserDto);
        bool VerifyUserLogPasses(string email, string password);
        string CreateJwtToken(User user);
    }
    public class AccountService : IAccountService
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IMapper mapper;
        private readonly AuthenticationSettings authenticationSettings;

        public AccountService(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, IMapper mapper, AuthenticationSettings authenticationSettings)
        {
            UnitOfWork = unitOfWork;
            this.passwordHasher = passwordHasher;
            this.mapper = mapper;
            this.authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterUserDto registerUserDto)
        {
            var newUser = mapper.Map<User>(registerUserDto);
            var hashedPassword = passwordHasher.HashPassword(newUser, registerUserDto.Password);
            newUser.PasswordHash = hashedPassword;
            AddUserTransaction addUserTransaction = CreateAddUserTransaction(newUser);
            addUserTransaction.Execute();
            UnitOfWork.Commit();

        }

        private AddUserTransaction CreateAddUserTransaction(User userToAdd)
        {
            switch (userToAdd.RoleId)
            {
                case 1:
                    throw new BadRequestException("Cannot creat admin.");
                case 2:
                    return new AddTeacherTransaction(UnitOfWork.Users, userToAdd);
                default:
                    return new AddStudentTransaction(UnitOfWork.Users, userToAdd);
            }
        }

        public string TryLoginUserAndGenerateJwt(LoginUserDto loginUserDto)
        {
            if(VerifyUserLogPasses(loginUserDto.Email, loginUserDto.Password))
            {
                var user = UnitOfWork.Users.Entity.Include(u => u.Role).FirstOrDefault(u => u.Email == loginUserDto.Email);
                var token = CreateJwtToken(user);
                return token;
            }
            else
            {
                throw new BadRequestException("Invalid email or password.");
            }
        }

        public bool VerifyUserLogPasses(string email, string password)
        {
            var user = UnitOfWork.Users.Entity.FirstOrDefault(u => u.Email == email);

            if (user is null)
            {
                return false;
            }
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public string CreateJwtToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Lastname),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer, authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
