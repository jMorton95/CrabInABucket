using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Mappers;
using FinanceManager.Common.Responses;
using FinanceManager.Data.Read.Friends;

namespace FinanceManager.Api.Features.Friendships;

public class FriendsList : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("friendslist", Handler)
        .WithDescription("Provides a list of a users friends, pending requests, recommended friends and random suggestions")
        .WithRequestValidation<Request>()
        .EnsureEntityExists<User>(x => x.UserId)
        .SelfOrAdminResource<User>(x => x.UserId);
    
    private record Request(Guid UserId, int NumberOfSuggestions);

    private record Response
    (
        List<NamedUserResponse> Friends,
        List<NamedUserResponse> PendingFriends,
        List<NamedUserResponse> SuggestedFriends,
        List<NamedUserResponse> RandomSuggestions
    );

    private class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Please supply a User ID");

            RuleFor(x => x.NumberOfSuggestions)
                .NotEmpty()
                .GreaterThan(0);
        }
    }

    private static async Task<Results<Ok<Response>, NotFound>> Handler(Request request, IReadUserFriends readUserFriends)
    {
        var userFriends = await readUserFriends.GetUserFriends(request.UserId);
        var pendingRequests = await readUserFriends.GetPendingFriendRequests(request.UserId);
        var suggestedFriends = await readUserFriends.GetRelatedFriends(request.UserId);
        var randomFriends = await readUserFriends.GetRandomFriendSuggestions(request.UserId, request.NumberOfSuggestions);

        var suggestedFriendIds = new HashSet<Guid>(suggestedFriends.Select(x => x.Id));
        var filteredRandomFriends = randomFriends.Where(x => !suggestedFriendIds.Contains(x.Id));
        
        List<List<User>> allRefs = [userFriends, pendingRequests, suggestedFriends, randomFriends];

        if (allRefs.TrueForAll(x => x.Count == 0))
        {
            return TypedResults.NotFound();
        }
        
        var response = new Response(
            userFriends.Select(x => x.ToNamedUserResponse()).ToList(),
            pendingRequests.Select(x => x.ToNamedUserResponse()).ToList(),
            suggestedFriends.Select(x => x.ToNamedUserResponse()).ToList(),
            filteredRandomFriends.Select(x => x.ToNamedUserResponse()).ToList()
        );

        return TypedResults.Ok(response);
    }
}