using FinanceManager.Core.Interfaces;
using FinanceManager.Core.Mappers;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data.Read.Users;
using FinanceManager.Services.Generic.Password;

namespace FinanceManager.Services.Handlers;

public interface ILoginHandler
{
    Task<LoginResponse?> Login(LoginRequest req);
}

public class LoginHandler(IReadUsers readUsers, IPasswordHasher passwordHasher, IUserTokenService userTokenService) : ILoginHandler
{
    public async Task<LoginResponse?> Login(LoginRequest req)
    {
        var attemptedUser = await readUsers.GetUserByEmailAsync(req.Username);

        if (attemptedUser == null || !passwordHasher.CheckPassword(req.Password, attemptedUser.Password))
        {
            return null;
        }
        
        var token = userTokenService.CreateTokenWithClaims(await userTokenService.GetUserClaims(attemptedUser));

        return new LoginResponse(token, attemptedUser.ToUserResponse());
    }
}