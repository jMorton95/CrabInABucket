using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Contracts;
using FinanceManager.Data.Read.Accounts;
using FinanceManager.Data.Write.Accounts;

namespace FinanceManager.Api.Features.Accounts;

public class Edit : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/edit", Handler)
        .WithSummary("Edits a user's account")
        .WithRequestValidation<Request>();

    public record Request(Guid Id, string AccountName);

    private record Response(bool Success, string Message): IPostResponse;

    public class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator(IReadAccounts readAccounts)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Please specify an Account to Edit.");

            RuleFor(x => x.AccountName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30)
                .WithMessage("Account Name must not be empty and be between 3 and 30 characters.");
        
            RuleFor(x => x.AccountName)
                .MustAsync(async (req, name, _) => await readAccounts.DoesUserAccountExist(name, req.Id) == false)
                .WithMessage("You already have an account with this name.");    
        }
    }

    private static async Task<Results<Ok<Response>, ValidationError, BadRequest<Response>>> Handler (
        Request request,
        IWriteAccounts writeAccounts
    )
    {
        var result = await writeAccounts.EditAsync(request.Id, request.AccountName);
        
        var response = new Response(Success: result, Message: result ? "" : $"Error editing account.");

        return TypedResults.Ok(response);
    }
}