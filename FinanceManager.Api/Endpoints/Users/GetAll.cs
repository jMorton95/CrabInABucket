using FinanceManager.Common.Constants;
using FinanceManager.Common.Mappers;
using FinanceManager.Data.Read.Users;

namespace FinanceManager.Api.Endpoints.Users;

public class GetAll : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/get", Handler)
        .RequireAuthorization(PolicyConstants.AdminRole)
        .WithDescription("Allows an Administrator to see a list of all users of the system");
    
    private record Response(List<UserProfile> Users);
    
    private static async Task<Results<Ok<Response>, ValidationError, NotFound>> Handler(IReadUsers readUsers)
    {
        var users = await readUsers.GetAllAsync();
        
        if (users is not { Count: > 0 })
        {
            return TypedResults.NotFound();
        } 
        
        var response = new Response(users.Select(x => x.ToUserResponse()).ToList());
        
        return TypedResults.Ok(response);
    }
}