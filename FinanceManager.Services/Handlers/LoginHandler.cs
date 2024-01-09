using FinanceManager.Core.Interfaces;
using FinanceManager.Core.Mappers;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Read.Users;
using FinanceManager.Services.Services;

namespace FinanceManager.Services.Handlers;

public interface ILoginHandler
{
    Task<LoginResponse?> Login(LoginRequest req);
}

public class LoginHandler(IReadUsers readUsers, IPasswordService passwordService, IUserTokenService userTokenService) : ILoginHandler
{
    public async Task<LoginResponse?> Login(LoginRequest req)
    {
        var attemptedUser = await readUsers.GetUserByEmailAsync(req.Username);

        if (attemptedUser == null || !passwordService.CheckPassword(req.Password, attemptedUser.Password))
        {
            return null;
        }
        
        var token = userTokenService.CreateTokenWithClaims(await userTokenService.GetUserClaims(attemptedUser));

        return new LoginResponse(token, attemptedUser.ToUserResponse());
    }
}