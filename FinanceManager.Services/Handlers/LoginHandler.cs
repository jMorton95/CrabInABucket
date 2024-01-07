using FinanceManager.Core.Mappers;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data;
using FinanceManager.Services.Services;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Services.Handlers;

public interface ILoginHandler
{
    Task<LoginResponse?> Login(LoginRequest req);
}

public class LoginHandler(DataContext db, IPasswordService passwordService, IUserTokenService userTokenService) : ILoginHandler
{
    public async Task<LoginResponse?> Login(LoginRequest req)
    {
        var attemptedUser = await db.User.FirstOrDefaultAsync(x => req.Username == x.Username);

        if (attemptedUser == null)
        {
            return null;
        }

        var authSuccess = passwordService.CheckPassword(req.Password, attemptedUser.Password);

        if (!authSuccess)
        {
            return null;
        }

        var userClaims = await userTokenService.GetUserClaims(attemptedUser);
        
        var token = userTokenService.CreateTokenWithClaims(userClaims);

        return new LoginResponse(token, attemptedUser.ToUserResponse());
    }
}