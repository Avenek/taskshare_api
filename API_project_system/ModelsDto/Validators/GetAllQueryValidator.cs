using FluentValidation;

namespace API_project_system.ModelsDto.Validators
{
    public class GetAllQueryValidator : AbstractValidator<GetAllQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15, 30, 50, 100 }; 
        public GetAllQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);

            RuleFor(r => r.PageSize).Custom((value, context) => {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", allowedPageSizes)}]");
                }
            });
        }
    }
}
