using FinanceManager.Core.Middleware.UserContext;
using Microsoft.AspNetCore.Builder;

namespace FinanceManager.Application.DependencyInjection;

public static class MiddlewareRegistration
{
    public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CurrentUserMiddleware>();
    }
}