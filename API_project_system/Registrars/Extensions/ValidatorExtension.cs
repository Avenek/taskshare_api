using FluentValidation;
using API_project_system.Repositories;
using System.Reflection;

namespace API_project_system.Registrars.Extensions
{
    public static class ValidatorExtension
    {
        public static void AddValidatorsFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var validatorTypes = assembly.GetTypes().Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.Namespace != null &&
                type.Namespace.EndsWith(".Validators"));

            foreach (var validatorType in validatorTypes)
            {
                var genericValidatorInterface = typeof(IValidator<>).MakeGenericType(validatorType);
                services.AddScoped(genericValidatorInterface, validatorType);
            }
        }
    }
}
