using API_project_system.Entities;
using API_project_system.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace API_project_system.Authorization
{
    public class UserEnrolledToCourseRequirementHandler : AuthorizationHandler<UserEnrolledToCourseRequirement, Course>
    {
        private readonly IUserContextService userContextService;
        private readonly ILogger<UserEnrolledToCourseRequirementHandler> logger;

        public UserEnrolledToCourseRequirementHandler(IUserContextService userContextService, ILogger<UserEnrolledToCourseRequirementHandler> logger)
        {
            this.userContextService = userContextService;
            this.logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserEnrolledToCourseRequirement requirement, Course course)
        {
            var userId = userContextService.GetUserId;
            if (course.EnrolledUsers.Any(u => u.Id == userId))
            {
                context.Succeed(requirement);
            }
            else
            {
                logger.LogInformation($"User: {userId} tried to access to course {course.Id}.");
            }

            return Task.CompletedTask;
        }
    }
}
