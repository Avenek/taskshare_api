using API_project_system.Entities;
using API_project_system.Services;
using Microsoft.AspNetCore.Authorization;

namespace API_project_system.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, IHasUserId>
    {
        private readonly IUserContextService userContextService;

        public ResourceOperationRequirementHandler(IUserContextService userContextService)
        {
            this.userContextService = userContextService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, IHasUserId userIdEntity)
        {
            if (requirement.ResourseOperation == ResourseOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = userContextService.GetUserId;
            if (userIdEntity.UserId == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
