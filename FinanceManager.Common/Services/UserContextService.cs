using FinanceManager.Common.Constants;
using FinanceManager.Common.Utilities;

namespace FinanceManager.Common.Services;
public record UserContext(DecodedAccessToken? UserAccessToken);

public interface IUserContextService
{
    UserContext? CurrentUser { get; }
    void SetCurrentUser(UserContext user);
    Guid? GetCurrentUserId();
    bool IsAccessTokenExpired();

    bool IsUserAdmin();
}

public class UserContextService : IUserContextService
{
    public UserContext? CurrentUser { get; private set; } = null;

    public void SetCurrentUser(UserContext user)
    {
        CurrentUser = user;
        
        if (IsAccessTokenExpired())
        {
            CurrentUser = null;
        }
    }

    public Guid? GetCurrentUserId()
    {
        return CurrentUser?.UserAccessToken?.UserId;
    }

    public bool IsAccessTokenExpired()
    {
        return CurrentUser != null && DateTime.UtcNow > CurrentUser.UserAccessToken?.ExpiryDate;
    }

    public bool IsUserAdmin()
    {
        return CurrentUser?.UserAccessToken?.Roles?.Contains(PolicyConstants.AdminRole) ?? false;
    }
}