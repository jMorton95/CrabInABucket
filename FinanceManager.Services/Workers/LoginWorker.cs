using FinanceManager.Core.Mappers;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data;
using FinanceManager.Services.Services.Interfaces;
using FinanceManager.Services.Workers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Services.Workers;

public class LoginWorker : ILoginWorker
{
    private readonly DataContext _db;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public LoginWorker(DataContext db, IPasswordService passwordService, ITokenService tokenService)
    {
        _db = db;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse?> Login(LoginRequest req)
    {
        var attemptedUser = await _db.User.FirstOrDefaultAsync(x => req.Username == x.Username);

        if (attemptedUser == null)
        {
            return null;
        }

        var authSuccess = _passwordService.CheckPassword(req.Password, attemptedUser.Password);

        if (!authSuccess)
        {
            return null;
        }

        var userClaims = await _tokenService.GetUserClaims(attemptedUser);
        
        var token = _tokenService.CreateTokenWithClaims(userClaims);

        return new LoginResponse(token, attemptedUser.ToUserResponse());
    }
}