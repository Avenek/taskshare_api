using Microsoft.AspNetCore.Authorization;

namespace API_project_system.Authorization
{
    public enum ResourseOperation
    {
        Create,
        Read,
        Update,
        Delete
    }
    public class ResourceOperationRequirement : IAuthorizationRequirement
    {
        public ResourceOperationRequirement(ResourseOperation resourseOperation)
        {
            ResourseOperation = resourseOperation;
        }

        public ResourseOperation ResourseOperation { get; }
    }
}
