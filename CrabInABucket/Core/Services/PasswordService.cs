using System.Security.Cryptography;
using System.Text;
using CrabInABucket.Core.Processes.Interfaces;
using CrabInABucket.Core.Services.Interfaces;
using CrabInABucket.Data;
using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Core.Services;

public class PasswordService : IPasswordService
{
    private readonly IPasswordProcess _passwordProcess;

    public PasswordService(IPasswordProcess passwordProcess)
    {
        _passwordProcess = passwordProcess;
    }
    
    public string HashPassword(string password)
    {
        var rng = RandomNumberGenerator.Create();
        var salt = _passwordProcess.CreateSalt();

        rng.GetBytes(salt);

        var passwordKey = _passwordProcess.CreatePasswordKey(password, salt);

        var hashedPassword = _passwordProcess.HashPassword(passwordKey, salt);
       

        return _passwordProcess.HashedPasswordToString(hashedPassword);

    }

    public bool CheckPassword(string loginPassword, string storedPassword)
    {
        var bytes = _passwordProcess.PasswordToBytes(storedPassword);

        var salt = _passwordProcess.ExtractSalt(bytes);

        var passwordKey = _passwordProcess.CreatePasswordKey(loginPassword, salt);

        var compared = _passwordProcess.ComparePassword(passwordKey, salt, bytes);

        return compared;
    }
}