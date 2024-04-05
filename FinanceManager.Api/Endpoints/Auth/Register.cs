using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Contracts;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Mappers;
using FinanceManager.Common.Services;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FinanceManager.Api.Endpoints.Auth;

public class Register : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/register", Handler)
        .WithSummary("Register a new user")
        .WithRequestValidation<Request>();

    public record Request(string Username, string Password, string PasswordConfirmation);

    public record Response(NamedUser? UserResponse, bool Success, string Message)
       : IPostResponse;

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator(IReadUsers readUsers)
        {
            RuleFor(x => x.Username)
                .EmailAddress()
                .NotEmpty()
                .MustAsync(async (x, _) => await readUsers.CheckUserExistsByEmail(x) == false)
                .WithMessage("Account with that Email Address already exists.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(20);

            RuleFor(x => x.PasswordConfirmation)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(20)
                .Equal(x => x.Password)
                .WithMessage("Passwords must match.");
        }
    }
    
    private static async Task<Results<Ok<Response>, ValidationError, BadRequest<Response>>> Handler(
        Request request,
        IReadUsers read,
        IWriteUsers write,
        IPasswordHasher passwordHasher 
    )
    {
        var password = passwordHasher.HashPassword(request.Password);
        
        var createResult = await write.CreateAsync(new User { Username = request.Username, Password = password });

        if (!createResult)
        {
            var errorResponse = new Response(null, false, "Error occurred creating account.");
            return TypedResults.BadRequest(errorResponse);
        }

        var userResult = await read.GetUserByEmailAsync(request.Username);

        if (userResult is null)
        {
            var errorResponse = new Response(null, false, $"Error occurred creating account for {request.Username}");
            return TypedResults.BadRequest(errorResponse);
        }

        var response = new Response(userResult.ToNamedUserResponse(), true, $"Successfully created account for {request.Username}");
        
        return TypedResults.Ok(response);
    }
}