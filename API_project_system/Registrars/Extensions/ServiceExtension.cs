using System.Reflection;

namespace API_project_system.Registrars.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServicesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var serviceTypes = assembly.GetTypes().Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.Namespace != null &&
                type.Namespace.EndsWith(".Services") &&
                type.GetInterfaces().Any(interfaceType =>
                    interfaceType.Name.EndsWith("Service")));

            foreach (var serviceType in serviceTypes)
            {
                var interfaceType = serviceType.GetInterfaces().FirstOrDefault();
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, serviceType);
                }
            }
        }
    }
}
