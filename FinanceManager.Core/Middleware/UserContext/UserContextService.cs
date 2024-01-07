using FinanceManager.Core.Utilities;

namespace FinanceManager.Services.Middleware.UserContext;

public interface IUserContextService
{
    UserContext? CurrentUser { get; }
    void SetCurrentUser(UserContext user);
}

public class UserContext
{
    public DecodedAccessToken? UserAccessToken { get; set; }

    public bool IsTokenExpired () => UserAccessToken != null && DateTime.UtcNow > UserAccessToken.ExpiryDate;
}

public class UserContextService : IUserContextService
{
    public UserContext? CurrentUser { get; private set; } = null;

    public void SetCurrentUser(UserContext user)
    {
        CurrentUser = user;
    }
}