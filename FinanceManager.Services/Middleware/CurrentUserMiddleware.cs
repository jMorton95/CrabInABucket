using FinanceManager.Services.Services;
using FinanceManager.Services.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FinanceManager.Services.Middleware;

public class CurrentUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserContextService userContextService, ITokenService tokenService)
    {
        var authorizationHeader = context.Request.Headers["Authorization"].ToString();
        
        if (!string.IsNullOrWhiteSpace(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader["Bearer".Length..].Trim();
            try
            {
                var decodedToken = tokenService.DecodeAccessToken(token);
                var user = new UserContext { UserId = decodedToken.UserId };

                userContextService.SetCurrentUser(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Caught: {ex}");
            }
        }

        await next(context);
    }
}