namespace FinanceManager.Services.Middleware.UserContext;

public interface IUserContextService
{
    UserContext? CurrentUser { get; }
    void SetCurrentUser(UserContext user);
}

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