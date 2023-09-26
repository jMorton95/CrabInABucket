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
        var salt = new byte[32];

        rng.GetBytes(salt);
        
        var passwordKey = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        
        var hashBytes = passwordKey.GetBytes(32);
        
        var bytePassword = new byte[salt.Length + hashBytes.Length];
        
        Array.Copy(salt, 0, bytePassword, 0, salt.Length);
        
        Array.Copy(hashBytes, 0, bytePassword, salt.Length, hashBytes.Length);
        
        var builder = new StringBuilder(bytePassword.Length * 2);
        
        foreach (var b in bytePassword)
        {
            builder.Append(b.ToString("x2"));
        }
        
        return builder.ToString();
        
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