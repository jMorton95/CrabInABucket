using FinanceManager.Core.Responses;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;
using FinanceManager.Services.Services.Interfaces;

namespace FinanceManager.Services.Services;

public class GrantAdministratorService(IReadUsers query, IWriteUsers write) : IGrantAdministratorService
{
    public async Task<PostResponse> GrantAdministrator(Guid userId)
    {
        var user = await query.GetByIdAsync(userId);

        if (user == null) return new PostResponse(false, "Could not find user.");

        var result = await write.GrantAdministratorRole(user);

        return result < 0 
            ? new PostResponse(false, "Error granting role.")
            : new PostResponse(true, "Success");
    }
}