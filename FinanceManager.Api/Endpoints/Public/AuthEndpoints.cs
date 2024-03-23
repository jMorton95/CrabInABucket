using FinanceManager.Common.Requests;
using FinanceManager.Common.Responses;
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
    }
    
    private static void MapRegisterEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/register", async Task<Results<Ok<BasePostResponse>, ValidationProblem>>(
            [FromBody] CreateUserRequest req,
            [FromServices] IValidator<CreateUserRequest> validator,
            [FromServices] ICreateUserHandler handler) => 
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