using FinanceManager.Common.Responses;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Users;

namespace FinanceManager.Services.Handlers;

public interface IRoleHandler
{
    Task<BasePostResponse> ChangeUserAdminRole(Guid userId, bool isAdmin);
}

public class RoleHandler(IReadUsers query, IWriteUsers write) : IRoleHandler
{
    public async Task<BasePostResponse> ChangeUserAdminRole(Guid userId, bool isAdmin)
    {
        var user = await query.GetByIdAsync(userId);

        if (user == null) return new BasePostResponse(false, "Could not find user.");
        
        return await write.ManageUserAdministratorRole(user, isAdmin) < 0 
            ? new BasePostResponse(false, "Error changing role.")
            : new BasePostResponse(true, "Success");
    }
}