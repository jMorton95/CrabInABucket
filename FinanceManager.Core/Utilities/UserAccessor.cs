using System.Security.Claims;
using System.Web;

namespace FinanceManager.Core.Utilities;

public interface IUserAccessor
{
    Guid? GetCurrentUserId();
}

public sealed class UserAccessor(IHttpContextAccessor http) : IUserAccessor
{
    public Guid? GetCurrentUserId()
    {
        var userId = http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        return userId != null ? Guid.Parse(userId.Value) : null;
    }
}