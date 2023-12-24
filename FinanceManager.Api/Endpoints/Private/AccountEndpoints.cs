using FinanceManager.Core.Mappers;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data;
using FinanceManager.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Api.Endpoints.Private;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var accountGroup = app.MapGroup("/api/account/")
            .WithTags("Account")
            .RequireAuthorization();

        accountGroup.MapPost("/create", async Task<Results<Ok<UserResponse>, BadRequest>> (
            [FromBody] CreateAccountRequest req,
            [FromServices] DataContext db,
            [FromServices] IUserContextService userContext
        ) => {
            var userId = userContext.CurrentUser?.UserId;

            if (userId == null)
            {
                return TypedResults.BadRequest();
            }
            
            var user = await db.User.FirstAsync(x => x.Id == userId);
          
            return TypedResults.Ok(user.ToUserResponse());
        });
    }    
}