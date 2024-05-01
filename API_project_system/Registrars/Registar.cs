using API_project_system.Entities;
using API_project_system.Registrars.Extensions;
using API_project_system.Repositories;
using API_project_system.Services;
using System.Reflection;

namespace API_project_system.Registrars
{
    public class Registar
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            AddServices(services, assembly);
        }

        public static void AddServices(IServiceCollection services, Assembly assembly)
        {
            services.AddServicesFromAssembly(assembly);
            services.AddRepositoriesFromAssembly(assembly);
            services.AddMappersFromAssembly(assembly);
            services.AddMainRegistarsFromAssembly(assembly);
        }
    }
}
