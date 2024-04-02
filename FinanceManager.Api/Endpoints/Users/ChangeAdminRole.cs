using FinanceManager.Api.RouteHandlers;
using FinanceManager.Common.Constants;
using FinanceManager.Common.Entities;
using FinanceManager.Common.Responses;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;

namespace FinanceManager.Api.Features.Users;

public class ChangeAdminRole : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("change-admin-role", Handler)
        .RequireAuthorization(PolicyConstants.AdminRole)
        .WithDescription("Add or remove Administrator privileges to a specified User")
        .WithRequestValidation<Request>()
        .EnsureEntityExists<User>(x => x.UserId);

    private record Request(Guid UserId, bool IsAdmin);

    private record Response(bool Success, string Message) : BasePostResponse(Success, Message);

    private class RequestValidator : AbstractValidator<Request>
    {
        public RequestValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.IsAdmin)
                .NotEmpty();
        }
    }

    private static async Task<Results<Ok<Response>, BadRequest<Response>>> Handler(
        Request request,
        IReadUsers readUsers,
        IWriteUsers writeUsers
    )
    {
        var user = await readUsers.GetByIdAsync(request.UserId)!;

        var roleChangeResult = await writeUsers.ManageUserAdministratorRole(user, request.IsAdmin);

        if (!roleChangeResult)
        {
            return TypedResults.BadRequest(new Response(roleChangeResult, $"Error occurred changing the role of {request.UserId}"));
        }

        var response = new Response(roleChangeResult, "Success");

        return TypedResults.Ok(response);
    }
}