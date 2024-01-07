using FinanceManager.Core.Middleware.UserContext;

namespace FinanceManager.Data;

public class AccessUserContext(IUserContextService userContextService)
{
    protected Guid? UserId = userContextService.GetCurrentUserId();
}