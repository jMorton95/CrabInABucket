using FinanceManager.Core.Models;
using FinanceManager.Data;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Endpoints.Private;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var accountGroup = app.MapGroup("/api/account/").WithTags("Account").RequireAuthorization();

        accountGroup.MapPost("/create", async ([FromServices] DataContext db) =>
        {
            var newAccount = new Account()
            {
                Name = "Josh's Account"
            };
            var account = db.Account.Add(newAccount);
            await db.SaveChangesAsync();
            
            return TypedResults.Ok();
        });
    }    
}