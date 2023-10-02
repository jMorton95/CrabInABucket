using FinanceManager.Core.Responses;

namespace FinanceManager.Services.Services.Interfaces;

public interface IGrantAdministratorService
{
    Task<PostResponse> GrantAdministrator(Guid userId);
}