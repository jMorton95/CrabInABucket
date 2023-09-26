using CrabInABucket.Api.Mappers;
using CrabInABucket.Api.Requests;
using CrabInABucket.Api.Responses;
using CrabInABucket.Core.Services.Interfaces;
using CrabInABucket.Core.Workers.Interfaces;
using CrabInABucket.Data;
using CrabInABucket.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Core.Workers;

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

        var token = await _tokenService.CreateToken(attemptedUser);

        return new LoginResponse(token, attemptedUser.ToUserResponse());
    }
}