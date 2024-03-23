using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;
using API_project_system;
using API_project_system.DbContexts;
using API_project_system.Entities;
using API_project_system.MappingProfiles;
using API_project_system.ModelsDto;
using API_project_system.ModelsDto.Validators;
using API_project_system.Repositories;
using API_project_system.Services;

namespace UnitTests
{
    [TestClass]
    public class AccountServiceTests
    {
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IAccountService service;
        private readonly IValidator<RegisterUserDto> registerValidator;
        private readonly IUnitOfWork unitOfWork;

        public AccountServiceTests()
        {
            unitOfWork = Helper.GetRequiredService<IUnitOfWork>();
            SeedData();
            service = Helper.GetRequiredService<IAccountService>();
            passwordHasher = Helper.GetRequiredService<IPasswordHasher<User>>();
            registerValidator = new RegisterUserDtoValidator(unitOfWork);


        }
        private void SeedData()
        {
            List<Role> roles = [new Role() { Name = "Admin" }, new Role() { Name = "Teacher" }, new Role() { Name = "Student" }];
            List<ApprovalStatus> statuses = [new ApprovalStatus() { Name = "Confirmed" }, new ApprovalStatus() { Name = "Needs confirmation" }];
            unitOfWork.Roles.AddRange(roles);
            unitOfWork.ApprovalStatuses.AddRange(statuses);
            unitOfWork.Commit();
        }

        [TestMethod]
        public void TryRegisterStudentWithValidatedData()
        {
            
                RegisterUserDto dto = new RegisterUserDto() { Email = "testUser@dto.pl", Name = "TestUser", Lastname = "TestUser", Password = "TestPassword", ConfirmedPassword = "TestPassword", RoleId = 3 };
                var validationResult = registerValidator.Validate(dto);
                Assert.AreEqual(true, validationResult.IsValid);
                service.RegisterUser(dto);
                var userInDatabase = service.UnitOfWork.Users.GetById(1);
                Assert.IsNotNull(userInDatabase);
                Assert.AreEqual("testUser@dto.pl", userInDatabase.Email);
                Assert.AreEqual("TestUser", userInDatabase.Name);
                Assert.AreEqual("TestUser", userInDatabase.Lastname);
                Assert.AreEqual(3, userInDatabase.RoleId);
                Assert.AreEqual(1, userInDatabase.StatusId);
                var result = passwordHasher.VerifyHashedPassword(userInDatabase, userInDatabase.PasswordHash, dto.Password);
                Assert.AreEqual(PasswordVerificationResult.Success, result);
        }

        [TestMethod]
        public void TryRegisterTeacherWithValidatedData()
        {
            RegisterUserDto dto = new RegisterUserDto() { Email = "testUser@dto.pl", Name = "TestUser", Lastname = "TestUser", Password = "TestPassword", ConfirmedPassword = "TestPassword", RoleId = 2 };
            var validationResult = registerValidator.Validate(dto);
            Assert.AreEqual(true, validationResult.IsValid);
            service.RegisterUser(dto);
            var userInDatabase = service.UnitOfWork.Users.GetById(1);
            Assert.IsNotNull(userInDatabase);
            Assert.AreEqual("testUser@dto.pl", userInDatabase.Email);
            Assert.AreEqual("TestUser", userInDatabase.Name);
            Assert.AreEqual("TestUser", userInDatabase.Lastname);
            Assert.AreEqual(2, userInDatabase.RoleId);
            Assert.AreEqual(2, userInDatabase.StatusId);
            var result = passwordHasher.VerifyHashedPassword(userInDatabase, userInDatabase.PasswordHash, dto.Password);
            Assert.AreEqual(PasswordVerificationResult.Success, result);
        }

        [TestMethod]
        public void TryRegisterStudentWithWrongPassword()
        {
            RegisterUserDto dto = new RegisterUserDto() { Email = "testUser@dto.pl", Name = "TestUser", Lastname = "TestUser", Password = "Test", ConfirmedPassword = "TestPassword", RoleId = 3 };
            var validationResult = registerValidator.Validate(dto);
            Assert.AreEqual(false, validationResult.IsValid);
        }

        [TestMethod]
        public void TryRegisterUserWithTakenEmail()
        {
            RegisterUserDto dto = new RegisterUserDto() { Email = "testUser@dto.pl", Name = "TestUser", Lastname = "TestUser", Password = "Test", ConfirmedPassword = "TestPassword" };
            service.RegisterUser(dto);
            RegisterUserDto takenEmailDto = new RegisterUserDto() { Email = "testUser@dto.pl", Name = "TestUser", Lastname = "TestUser", Password = "Test", ConfirmedPassword = "TestPassword" };
            var validationResult = registerValidator.Validate(takenEmailDto);
            Assert.AreEqual(false, validationResult.IsValid);
        }

        [TestMethod]
        public void TryRegisterUserWithTakenNicnkane()
        {
            RegisterUserDto dto = new RegisterUserDto() { Email = "test@dto.pl", Name = "TestUser", Lastname = "TestUser", Password = "Test", ConfirmedPassword = "TestPassword" };
            service.RegisterUser(dto);
            RegisterUserDto takenEmailDto = new RegisterUserDto() { Email = "testUser@dto.pl", Name = "TestUser", Lastname = "TestUser", Password = "Test", ConfirmedPassword = "TestPassword" };
            var validationResult = registerValidator.Validate(takenEmailDto);
            Assert.AreEqual(false, validationResult.IsValid);
        }

        [TestMethod]
        public void TryRegisterUserWithInvalidEmail()
        {
            RegisterUserDto dto = new RegisterUserDto() { Email = "test", Name = "TestUser", Lastname = "TestUser", Password = "Test", ConfirmedPassword = "TestPassword" };
            var validationResult = registerValidator.Validate(dto);
            Assert.AreEqual(false, validationResult.IsValid);
        }

        [TestMethod]
        public void TestVerifyingUserLogPassesWithCorrectLogPasses()
        {
            RegisterUserDto registerDto = new RegisterUserDto() { Email = "testUser@dto.pl", Name = "TestUser", Lastname = "TestUser", Password = "TestPassword", ConfirmedPassword = "TestPassword" };
            service.RegisterUser(registerDto);
            string email = "testUser@dto.pl";
            string password = "TestPassword";
            bool result = service.VerifyUserLogPasses(email, password);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestVerifyingUserLogPassesWithWrongEmail()
        {
            RegisterUserDto registerDto = new RegisterUserDto() { Email = "testUser@dto.pl", Name = "TestUser", Lastname = "TestUser", Password = "TestPassword", ConfirmedPassword = "TestPassword" };
            service.RegisterUser(registerDto);
            string email = "test@dto.pl";
            string password = "TestPassword";
            bool result = service.VerifyUserLogPasses(email, password);
            Assert.AreEqual(false, result);
        }
        [TestMethod]
        public void TestVerifyingUserLogPassesWithWrongPassword()
        {
            RegisterUserDto registerDto = new RegisterUserDto() { Email = "testUser@dto.pl", Name = "TestUser", Lastname = "TestUser", Password = "TestPassword", ConfirmedPassword = "TestPassword" };
            service.RegisterUser(registerDto);
            string email = "testUser@dto.pl";
            string password = "Test";
            bool result = service.VerifyUserLogPasses(email, password);
            Assert.AreEqual(false, result);
        }
       
    }
}