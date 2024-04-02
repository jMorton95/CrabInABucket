using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Mappers;
using FinanceManager.Common.Responses;
using FinanceManager.Common.Services;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;
using Microsoft.AspNetCore.Identity;

namespace FinanceManager.Api.Features.Auth;

public class Login : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/login", Handler)
        .WithSummary("Login with Username & Password")
        .WithRequestValidation<Request>();

    private record Request(string Username, string Password);

    private record Response(TokenWithExpiryResponse AccessToken, UserResponse User);

    private class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.Username)
                .EmailAddress()
                .NotEmpty();
            
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .MaximumLength(20)
                .NotEmpty();
        }
    }

    private static async Task<Results<Ok<Response>, UnauthorizedHttpResult>> Handler(
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
        
        var token = userTokenService.CreateTokenWithClaims(await userTokenService.GetUserClaims(attemptedUser));
        
        var response = new Response(token, attemptedUser.ToUserResponse());
        
        return TypedResults.Ok(response);
    }

}