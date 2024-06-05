using API_project_system.Repositories;
using System.Reflection;

namespace API_project_system.Registrars.Extensions
{
    public static class MapperExtension
    {
        public static void AddMappersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var mapperTypes = assembly.GetTypes().Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.Namespace != null &&
                type.Namespace.EndsWith(".MappingProfiles"));

            foreach (var mapperType in mapperTypes)
            {
                services.AddAutoMapper(mapperType);
            }
        }
    }
}
