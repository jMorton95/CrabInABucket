using System.Security.Cryptography;
using FinanceManager.Services.Generic.Password;

namespace FinanceManager.Services.Services;

public interface IPasswordService
{
    string HashPassword(string password);

    bool CheckPassword(string loginPassword, string storedPassword);
}

public class PasswordService : IPasswordService
{
    private readonly IPasswordUtilities _passwordUtilities;

    public PasswordService(IPasswordUtilities passwordUtilities)
    {
        _passwordUtilities = passwordUtilities;
    }
    
    public string HashPassword(string password)
    {
        var rng = RandomNumberGenerator.Create();
        var salt = _passwordUtilities.CreateSalt();

        rng.GetBytes(salt);

        var passwordKey = _passwordUtilities.CreatePasswordKey(password, salt);

        var hashedPassword = _passwordUtilities.HashPassword(passwordKey, salt);
        
        return _passwordUtilities.HashedPasswordToString(hashedPassword);

    }

    public bool CheckPassword(string loginPassword, string storedPassword)
    {
        var bytes = _passwordUtilities.PasswordToBytes(storedPassword);

        var salt = _passwordUtilities.ExtractSalt(bytes);

        var passwordKey = _passwordUtilities.CreatePasswordKey(loginPassword, salt);

        var compared = _passwordUtilities.ComparePassword(passwordKey, salt, bytes);

        return compared;
    }
}