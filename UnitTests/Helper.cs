using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using API_project_system.DbContexts;
using API_project_system.Entities;
using API_project_system.MappingProfiles;
using API_project_system.Middleware;
using API_project_system.ModelsDto.Validators;
using API_project_system.ModelsDto;
using API_project_system.Repositories;
using API_project_system.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using API_project_system;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Moq;
using API_project_system.Registrars;
namespace UnitTests
{

    internal class Helper
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        private static IServiceProvider Provider()
        {
            var services = new ServiceCollection();

            services.AddDbContext<SystemDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            AddServices(services);

            return services.BuildServiceProvider();
        }

        public static T GetRequiredService<T>()
        {
            var provider = Provider();
            return provider.GetRequiredService<T>();
        }

        private static void AddServices(IServiceCollection services)
        {
            Registar registar = new Registar();
            registar.ConfigureServices(services);
        }

        public static IUnitOfWork CreateUnitOfWork()
        {
            var dbContext = GetRequiredService<SystemDbContext>();
            var roles = GetRoles();
            dbContext.Roles.AddRange(roles);
            dbContext.SaveChanges();
            return new UnitOfWork(dbContext) { UserId = 1 };
        }

        public static IUserContextService CreateMockIUserContextService()
        {
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, "1")
            }));

            httpContextAccessorMock.Setup(x => x.HttpContext.User).Returns(user);

            var userContextService = new UserContextService(httpContextAccessorMock.Object);

            return userContextService;
        }

        public static void ChangeIdInIUserContextService(IUserContextService userContextService, int userId)
        {

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }));

            userContextService.User = user;
        }

        private static IEnumerable<Role> GetRoles()
        {
            List<Role> roles = [new Role() { Name = "Admin" }, new Role() { Name = "Teacher" }, new Role() { Name = "Student" }];
            return roles;
        }

        public static void RegisterAccount(IAccountService accountService, string email = "testUser@dto.pl", string name = "TestUser", string lastName = "UserLast", string password = "TestPassword")
        {
            RegisterUserDto registerDto = new RegisterUserDto() { Email = email, Name = name, Lastname = lastName, Password = password, ConfirmedPassword = password };
            accountService.RegisterAccount(registerDto);
        }
    }
}
