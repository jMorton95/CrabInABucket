using FinanceManager.Core.Responses;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;

namespace FinanceManager.Services.Services;

public interface IRoleService
{
    Task<BasePostResponse> ChangeUserAdminRole(Guid userId, bool isAdmin);
}

public class RoleService(IReadUsers query, IWriteUsers write) : IRoleService
{
    public async Task<BasePostResponse> ChangeUserAdminRole(Guid userId, bool isAdmin)
    {
        var user = await query.GetByIdAsync(userId);

        if (user == null) return new BasePostResponse(false, "Could not find user.");

        var result = isAdmin
            ? await write.GrantAdministratorRole(user) 
            : await write.RemoveAdministratorRole(user);

        return result < 0 
            ? new BasePostResponse(false, "Error changing role.")
            : new BasePostResponse(true, "Success");
    }
}