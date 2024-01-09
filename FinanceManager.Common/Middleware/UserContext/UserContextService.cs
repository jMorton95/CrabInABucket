using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Utilities;

namespace FinanceManager.Core.Middleware.UserContext;
public record UserContext(DecodedAccessToken? UserAccessToken);

public interface IUserContextService
{
    UserContext? CurrentUser { get; }
    void SetCurrentUser(UserContext user);
    Guid? GetCurrentUserId();
    bool IsAccessTokenExpired();
}

public class UserContextService() : IUserContextService
{
    public UserContext? CurrentUser { get; private set; } = null;

    public void SetCurrentUser(UserContext user)
    {
        CurrentUser = user;
    }

    public Guid? GetCurrentUserId()
    {
        return CurrentUser?.UserAccessToken?.UserId;
    }

    public bool IsAccessTokenExpired()
    {
        return CurrentUser != null && DateTime.UtcNow > CurrentUser.UserAccessToken?.ExpiryDate;
    }
}