using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Contracts;
using FinanceManager.Common.Entities;
using FinanceManager.Data.Read.Accounts;
using FinanceManager.Data.Write.Accounts;

namespace FinanceManager.Api.Features.Accounts;

public class Create : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/create", Handler)
        .WithSummary("Create a new account for a user")
        .WithRequestValidation<Request>();

    private record Request(Guid UserId, string AccountName);

    private record Response(bool Success, string Message): IPostResponse;

    private class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
            
            RuleFor(x => x.AccountName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30)
                .WithMessage("Account Name must not be between 3 and 30 characters.");
        }
    }
    
    private static async Task<Results<Ok<Response>, BadRequest<Response>>> Handler (
        Request request,
        IReadAccounts readAccounts,
        IWriteAccounts writeAccounts
    )
    {
        var accountExists = await readAccounts.DoesUserAccountExist(request.AccountName, request.UserId);

        if (accountExists)
        {
            var errorResponse = new Response(false, $"Account with name: {request.AccountName} already exists.");
            return TypedResults.BadRequest(errorResponse);
        }
        
        var creationResult = await writeAccounts.CreateAsync(new Account() { Name = request.AccountName });

        var response = new Response(creationResult, creationResult ? "" : "Could not create account");

        return response.Success ? TypedResults.Ok(response) : TypedResults.BadRequest(response);
    }
}