using FinanceManager.Common.Entities;

namespace FinanceManager.Api.RouteHandlers.Filters;

public class EnsureEntityExistsFilter<TRequest, TEntity>(Func<TRequest, Guid?> entityId) : IEndpointFilter 
    where TEntity : class, IEntity
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.Arguments.OfType<TRequest>().Single();
        var ct = context.HttpContext.RequestAborted;
        var id = entityId(request);

        if (!id.HasValue)
        {
            return await next(context);
        }

        var db = context.HttpContext.RequestServices.GetRequiredService<DataContext>();

        var exists = await db.Set<TEntity>().AnyAsync(x => x.Id == id, ct);

        if (!exists)
        {
            return TypedResults.Problem
            (
                statusCode: StatusCodes.Status404NotFound,
                title: "Not Found",
                detail: $"{typeof(TEntity).Name} with id {id} was not found."
            );
        }
        
        return await next(context);
    }
}