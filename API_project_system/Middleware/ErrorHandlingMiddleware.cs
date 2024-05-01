
using API_project_system.Exceptions;
using API_project_system.Services;

namespace API_project_system.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserContextService userContextService;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, IUnitOfWork unitOfWork, IUserContextService userContextService)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
            this.userContextService = userContextService;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                var jwtToken = context.Request.Headers["Authorization"].ToString()?.Split(" ").LastOrDefault();
                if (!string.IsNullOrEmpty(jwtToken))
                {
                    if (unitOfWork.BlackListedTokens.Entity.Any(token => token.Token.Equals(jwtToken)))
                    {
                        throw new ForbidException("Unauthorized: Token is blacklisted.");
                    }
                    var userId = userContextService.GetUserId;
                    unitOfWork.UserId = userId;
                }

                await next.Invoke(context);
            }
            catch (ForbidException e)
            {
                logger.LogError(e, e.Message);
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(e.Message);
            }
            catch (NotFoundException e)
            {
                logger.LogError(e, e.Message);
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(e.Message);
            }
            catch (BadRequestException e)
            {
                logger.LogError(e, e.Message);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong.");
            }
        }
    }
}
