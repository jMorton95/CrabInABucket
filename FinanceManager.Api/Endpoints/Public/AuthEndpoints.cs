using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Services.Handlers;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Endpoints.Public;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var authGroup = app.MapGroup("/api/auth/").WithTags("Auth").AllowAnonymous();
        
        authGroup.MapPost("/login", async Task<Results<Ok<LoginResponse>, ValidationProblem, BadRequest>>
        (
            [FromBody] LoginRequest req,
            [FromServices] IValidator<LoginRequest> validator,
            [FromServices] ILoginHandler handler
        )
            =>
        {
            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var res = await handler.Login(req);

            return res != null ? TypedResults.Ok(res) : TypedResults.BadRequest();
        });
        
        authGroup.MapPost("/register", async Task<Results<Ok<BasePostResponse>, ValidationProblem>>
        (
            [FromBody] CreateUserRequest req,
            [FromServices] IValidator<CreateUserRequest> validator,
            [FromServices] ICreateUserHandler handler
        ) 
            =>
        {
            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.CreateUser(req);

            return TypedResults.Ok(result);
        });
    }
}