using FluentValidation;
using API_project_system.Entities;
using API_project_system.Repositories;

namespace API_project_system.ModelsDto.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly IUnitOfWork unitOfWork;

        public RegisterUserDtoValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Custom((value, context) =>
                {
                     bool emailInUse = unitOfWork.Users.Entity.Any(u => u.Email == value);
                     if (emailInUse)
                     {
                         context.AddFailure("Email", "That email is taken.");
                     }
                });

            RuleFor(x => x.Name)
                .MinimumLength(3);

            RuleFor(x => x.Lastname)
                .MinimumLength(3);

            RuleFor(x => x.Password)
                .MinimumLength(6);

            RuleFor(x => x.ConfirmedPassword)
                .Equal(e =>  e.Password);

            RuleFor(x => x.RoleId)
                .Custom((value, context) =>
                {
                    bool isCorrectRole = value > 0 && value <= unitOfWork.Roles.Entity.Count();
                    if (!isCorrectRole)
                    {
                        context.AddFailure("RoleId", "That role doesn't exist.");
                    }
                });
        }
    }
}
