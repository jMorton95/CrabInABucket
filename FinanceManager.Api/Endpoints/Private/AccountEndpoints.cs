using FinanceManager.Core.Requests;
using FinanceManager.Data;
using FinanceManager.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Api.Endpoints.Private;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var accountGroup = app.MapGroup("/api/account/").WithTags("Account").RequireAuthorization();

        accountGroup.MapPost("/create", async ([FromBody] CreateAccountRequest req, [FromServices] DataContext db, [FromServices] IHttpContextAccessor context, [FromServices] ITokenService tokenService) =>
        {
            var authorizationHeader = context.HttpContext.Request.Headers.Authorization.ToString();
            
            var token = authorizationHeader["Bearer ".Length..].Trim();
            
            var decodedToken = tokenService.DecodeAccessToken(token);

            var user = await db.User.FirstAsync(x => x.Id == decodedToken.UserId);
          
            return TypedResults.Ok(user);
        });
    }    
}