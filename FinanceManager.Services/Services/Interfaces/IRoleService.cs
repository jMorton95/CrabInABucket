using FinanceManager.Core.Responses;

namespace FinanceManager.Services.Services.Interfaces;

public interface IRoleService
{
    Task<PostResponse> ChangeUserAdminRole(Guid userId, bool isAdmin);
}