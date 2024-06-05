using API_project_system.Repositories;
using System.Reflection;

namespace API_project_system.Registrars.Extensions
{
    public static class RepositoryExtensions
    {
        public static void AddRepositoriesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var entityTypes = assembly.GetTypes().Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.Namespace != null &&
                type.Namespace.EndsWith(".Entities"));

            foreach (var entityType in entityTypes)
            {
                var genericRepositoryInterface = typeof(IRepository<>).MakeGenericType(entityType);
                var genericRepositoryType = typeof(Repository<>).MakeGenericType(entityType);
                services.AddScoped(genericRepositoryInterface, genericRepositoryType);
            }
        }
    }
}
