using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FinanceManager.Core.Utilities;

public interface IUserAccessor
{
    Guid? GetCurrentUserId();
}

public sealed class UserAccessor(IHttpContextAccessor http) : IUserAccessor
{
    public Guid? GetCurrentUserId()
    {
        //http.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        var userId = http.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        return userId != null ? Guid.Parse(userId.Value) : null;
    }
}