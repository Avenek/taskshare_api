using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using API_project_system.Entities;
using API_project_system.Middleware;
using API_project_system.Seeders;
using System.Reflection;
using API_project_system.Logger;
using API_project_system.Authorization;

namespace API_project_system.Registrars.Extensions
{
    public static class MainExtension
    {
        public static void AddMainRegistars(this IServiceCollection services)
        {
            services.AddScoped<Seeder>();
            services.AddScoped<JwtTokenHelper>();
            services.AddScoped<UserActionLogger>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler> ();

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        }
    }
}
