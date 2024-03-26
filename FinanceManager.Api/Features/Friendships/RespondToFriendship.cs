using FinanceManager.Common.DataEntities;
using FinanceManager.Common.Responses;
using FinanceManager.Common.RouteHandlers;
using FinanceManager.Data.Write.Friendships;

namespace FinanceManager.Api.Features.Friendships;

public class RespondToFriendship : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/respond", Handler)
        .WithDescription("Respond to a friendship request")
        .WithRequestValidation<Request>()
        .EnsureEntityExists<User>(x => x.ResponderId)
        .EnsureEntityExists<Friendship>(x => x.FriendshipId)
        .EnsureRequestedUserIsCurrentUser<User>(x => x.ResponderId);

    private record Request(Guid FriendshipId, Guid ResponderId, bool Accepted);

    private record Response(bool Success, string Message) : BasePostResponse(Success, Message);

    private class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.FriendshipId)
                .NotEmpty()
                .WithMessage("Error occurred processing Friendship Request");
            
            RuleFor(x => x.ResponderId)
                .NotEmpty()
                .WithMessage("Error occurred accessing your User Id");
            
            RuleFor(x => x.Accepted)
                .NotNull()
                .WithMessage("Error occurred parsing your decision.");
        }
    }
    
    private static async Task<Results<Ok<Response>, BadRequest<Response>>> Handler(Request request, IWriteFriendships writeFriendships)
    {
        var result = await writeFriendships.EditAsync(request.FriendshipId, request.Accepted);
        
        var response = new Response(result, result ? "Success" : "Unable to confirm friendship decision.");
        
        return result ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}