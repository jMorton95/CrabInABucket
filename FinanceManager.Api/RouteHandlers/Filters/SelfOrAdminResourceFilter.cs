using FinanceManager.Common.Entities;
using FinanceManager.Common.Services;
using FinanceManager.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Api.RouteHandlers.Filters;

public class SelfOrAdminResourceFilter<TRequest, TEntity>(Func<TRequest, Guid?> requestedUserId) : IEndpointFilter 
    where TEntity : class, IEntity
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var currentUserService = context.HttpContext.RequestServices.GetRequiredService<IUserContextService>();
        var currentUserId = currentUserService.GetCurrentUserId();
        
        var request = context.Arguments.OfType<TRequest>().Single();
        var ct = context.HttpContext.RequestAborted;
        var id = requestedUserId(request);

        if (!id.HasValue || currentUserId is null)
        {
            return await next(context);
        }

        var db = context.HttpContext.RequestServices.GetRequiredService<DataContext>();

        var user = await db.User.FirstOrDefaultAsync(x => x.Id == id, ct);
        
        if (user is null)
        {
            return TypedResults.Problem
            (
                statusCode: StatusCodes.Status404NotFound,
                title: "Not Found",
                detail: $"{typeof(TEntity).Name} with id {id} was not found."
            );
        }
        
        if (user.Id != currentUserId || currentUserService.IsUserAdmin())
        {
            return TypedResults.Problem
            (
                statusCode: StatusCodes.Status403Forbidden,
                title: "Forbidden",
                detail: $"User with id {currentUserId} not allowed to modify asset owned by {user.Id}."
            );
        }
        
        return await next(context);
    }
}