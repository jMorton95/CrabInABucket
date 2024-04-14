using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Mappers;
using FinanceManager.Common.Models;
using FinanceManager.Common.Services;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;

namespace FinanceManager.Api.Endpoints.Auth;

public class Login : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/login", Handler)
        .WithSummary("Login with Username & Password")
        .WithRequestValidation<Request>();

    public record Request(string Username, string Password);

    public record Response(TokenWithExpiry AccessToken, UserProfile User);

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Username)
                .EmailAddress()
                .NotEmpty()
                .MaximumLength(100);
            
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .MaximumLength(20)
                .NotEmpty();
        }
    }

    private static async Task<Results<Ok<Response>, ValidationError, UnauthorizedHttpResult>> Handler(
        Request request,
        IReadUsers readUsers,
        IPasswordHasher passwordHasher,
        IWriteUsers writeUsers,
        IUserTokenService userTokenService,
        CancellationToken ct)
    {
        var attemptedUser = await readUsers.GetUserByEmailAsync(request.Username);

        if (attemptedUser == null || !passwordHasher.CheckPassword(request.Password, attemptedUser.Password))
        {
            return TypedResults.Unauthorized();
        }

        await writeUsers.UpdateLastLogin(attemptedUser);
        
        var token = userTokenService.CreateTokenWithClaims(userTokenService.GetUserClaims(attemptedUser));
        
        var response = new Response(token, attemptedUser.ToUserResponse());
        
        return TypedResults.Ok(response);
    }

}