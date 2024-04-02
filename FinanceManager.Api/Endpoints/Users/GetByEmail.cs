using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Constants;
using FinanceManager.Common.Mappers;
using FinanceManager.Data.Read.Users;

namespace FinanceManager.Api.Features.Users;

public class GetByEmail : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("get/{email}", Handler)
        .RequireAuthorization(PolicyConstants.AdminRole)
        .WithDescription("Search for a user by email address")
        .WithRequestValidation<Request>();

    private record Request(string Email);

    private record Response(UserProfile User);

    private class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Please enter an email address");
        }
    }

    private static async Task<Results<Ok<Response>, NotFound>> Handler([AsParameters] Request request, IReadUsers readUsers)
    {
        var user = await readUsers.GetUserByEmailAsync(request.Email);

        if (user is null)
        {
            return TypedResults.NotFound();
        }

        var response = new Response(user.ToUserResponse());

        return TypedResults.Ok(response);
    }

}