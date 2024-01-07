using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Endpoints.Private;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var accountGroup = app.MapGroup("/api/account/")
            .WithTags("Account")
            .RequireAuthorization();

        // accountGroup.MapPost("/create", async Task<Results<Ok<UserResponse>, BadRequest>> (
        //     [FromBody] CreateAccountRequest req
        // ) => {
        //    
        //     return TypedResults.Ok(0);
        // });
    }    
}