using FinanceManager.Common.Services;

namespace FinanceManager.Api.Middleware.UserContext;

public class CurrentUserMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IUserContextService userContextService, IUserTokenService userTokenService)
    {
        var authorizationHeader = context.Request.Headers.Authorization.ToString();
        
        if (!string.IsNullOrWhiteSpace(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
        {
            var token = authorizationHeader["Bearer".Length..].Trim();
            try
            {
                var decodedToken = userTokenService.DecodeAccessToken(token);
                var user = new Common.Services.UserContext(decodedToken);

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