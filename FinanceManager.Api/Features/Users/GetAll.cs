using FinanceManager.Common.Constants;
using FinanceManager.Common.Mappers;
using FinanceManager.Common.Responses;
using FinanceManager.Data.Read.Users;

namespace FinanceManager.Api.Features.Users;

public class GetAll : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/get", Handler)
        .RequireAuthorization(PolicyConstants.AdminRole)
        .WithDescription("Allows an Administrator to see a list of all users of the system");
    
    private record Response(List<UserResponse> Users);
    
    private static async Task<Results<Ok<List<Response>>, NotFound>> Handler(IReadUsers readUsers)
    {
        var users = await readUsers.GetAllAsync();

        if (users is not { Count: > 0 })
        {
            return TypedResults.NotFound();
        } 
        
        var userResponses = users.Select(x => x.ToUserResponse()).ToList();

        if (userResponses is not { Count: > 0 })
        {
            return TypedResults.NotFound();
        }

        var response = new Response(userResponses);
        
        return response != null ? TypedResults.Ok(response) : TypedResults.NotFound();
    }
}