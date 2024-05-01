using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using API_project_system.Entities;
using API_project_system.Middleware;
using API_project_system.Seeders;
using System.Reflection;
using API_project_system.Logger;

namespace API_project_system.Registrars.Extensions
{
    public static class MainExtension
    {
        public static void AddMainRegistarsFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            services.AddScoped<Seeder>();
            services.AddScoped<JwtTokenHelper>();
            services.AddScoped<UserActionLogger>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHttpContextAccessor();

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        }
    }
}
