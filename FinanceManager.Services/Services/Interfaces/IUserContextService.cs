using FinanceManager.Services.Middleware;

namespace FinanceManager.Services.Services.Interfaces;

public interface IUserContextService
{
    UserContext? CurrentUser { get; }
    void SetCurrentUser(UserContext user);
}