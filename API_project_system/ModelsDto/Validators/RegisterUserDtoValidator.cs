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
                    bool emailInUse = unitOfWork.Users.Entity.Any(u => u.Email.ToString() == value);
                    if(emailInUse)
                    {
                        context.AddFailure("Email", "That email is taken.");
                    }
                });

            RuleFor(x => x.Nickname)
                .MinimumLength(3)
                .Custom((value, context) =>
                {
                    bool nicknameInUse = unitOfWork.Users.Entity.Any(u => u.Nickname.ToString() == value);
                    if (nicknameInUse)
                    {
                        context.AddFailure("Nickname", "That nickname is taken.");
                    }
                });

            RuleFor(x => x.Password)
                .MinimumLength(6);

            RuleFor(x => x.ConfirmedPassword)
                .Equal(e =>  e.Password);
        }
    }
}
