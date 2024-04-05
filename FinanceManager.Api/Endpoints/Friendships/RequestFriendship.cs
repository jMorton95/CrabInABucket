using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Contracts;
using FinanceManager.Data.Read.Friendships;
using FinanceManager.Data.Write.Friendships;

namespace FinanceManager.Api.Endpoints.Friendships;

public class RequestFriendship : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/request", Handler)
        .WithDescription("Creates a friendship entity, in a pending state, between two users.")
        .WithRequestValidation<Request>()
        .EnsureEntityExists<Common.Entities.User>(x => x.UserId)
        .EnsureEntityExists<Common.Entities.User>(x => x.TargetUserId);

    public record Request(Guid UserId, Guid TargetUserId);

    private record Response(bool Success, string Message): IPostResponse;
    
    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator(IReadFriendships readFriendships)
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Error occurred providing your User Id.");
            
            RuleFor(x => x.TargetUserId)
                .NotEmpty()
                .WithMessage("You must provide a valid person to request friendship.")
                .MustAsync(async (x, _) => await readFriendships.AreUsersFriends(x) != true)
                .WithMessage("You've already requested to be friends with that person.");
        }
    }

    private static async Task<Results<Ok<Response>, ValidationError, BadRequest<Response>>> Handler(Request request, IWriteFriendships write)
    {
        var createResult = await write.CreateAsync(request.UserId, request.TargetUserId);

        var response = new Response(createResult, createResult ? "Success" : "Error requesting friendship");

        return createResult ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}