using System.Security.Claims;
using CrabInABucket.Core.Services.Interfaces;

namespace CrabInABucket.Core.Services;

public sealed class UserAccessorService(IHttpContextAccessor http) : IUserAccessorService
{
    public Guid? GetCurrentUserId()
    {
        var userId = http.HttpContext?.User.FindFirstValue((ClaimTypes.NameIdentifier));
        return userId != null ? Guid.Parse(userId) : null;
    }
}