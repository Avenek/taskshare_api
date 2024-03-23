using FluentValidation;
using API_project_system.Entities;
using API_project_system.Repositories;

namespace API_project_system.ModelsDto.Validators
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty();

        }
    }
}
