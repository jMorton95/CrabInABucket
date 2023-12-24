using FinanceManager.Services.Services.Interfaces;

namespace FinanceManager.Services.Services;

public class UserContext
{
    public Guid UserId { get; set; }
}

public class UserContextService : IUserContextService
{
    public UserContext? CurrentUser { get; private set; } = null;

    public void SetCurrentUser(UserContext user)
    {
        CurrentUser = user;
    }
}