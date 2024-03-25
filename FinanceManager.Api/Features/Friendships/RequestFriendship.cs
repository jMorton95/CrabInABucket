using FinanceManager.Common.DataEntities;
using FinanceManager.Common.Responses;
using FinanceManager.Common.RouteHandlers;
using FinanceManager.Data.Read.Friendships;
using FinanceManager.Data.Write.Friendships;

namespace FinanceManager.Api.Features.Friendships;

public class RequestFriendship : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/request", Handler)
        .WithDescription("Creates a friendship entity, in a pending state, between two users.")
        .WithRequestValidation<Request>()
        .EnsureEntityExists<User>(x => x.UserId)
        .EnsureEntityExists<User>(x => x.TargetUserId);

    private record Request(Guid UserId, Guid TargetUserId);

    private record Response(bool Success, string Message) : BasePostResponse(Success, Message);
    
    private class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator(IReadFriendships readFriendships)
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Error occurred providing your User Id to request.");
            
            RuleFor(x => x.TargetUserId)
                .NotEmpty()
                .WithMessage("You must provide a valid person to request friendship.")
                .MustAsync(async (x, _) => await readFriendships.DoesFriendshipExist(x) != true)
                .WithMessage("You've already requested to be friends with that person.");
        }
    }

    private static async Task<Results<Ok<Response>, BadRequest<Response>>> Handler(Request request, IWriteFriendships write)
    {
        var createResult = await write.CreateAsync(request.UserId, request.TargetUserId);

        var response = new Response(createResult, createResult ? "Success" : "Error requesting friendship");

        return createResult ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}