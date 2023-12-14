using FinanceManager.Core.Responses;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;
using FinanceManager.Services.Services.Interfaces;

namespace FinanceManager.Services.Services;

public class RoleService(IReadUsers query, IWriteUsers write) : IRoleService
{
    public async Task<PostResponse> ChangeUserAdminRole(Guid userId, bool isAdmin)
    {
        var user = await query.GetByIdAsync(userId);

        if (user == null) return new PostResponse(false, "Could not find user.");

        var result = isAdmin
            ? await write.GrantAdministratorRole(user) 
            : await write.RemoveAdministratorRole(user);

        return result < 0 
            ? new PostResponse(false, "Error changing role.")
            : new PostResponse(true, "Success");
    }
}