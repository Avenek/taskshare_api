using API_project_system.ModelsDto.CourseDto;
using FluentValidation;

namespace API_project_system.ModelsDto.Validators
{
    public class AddCourseDtoValidator : AbstractValidator<AddCourseDto>
    {
        public AddCourseDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3);
        }
    }
}
