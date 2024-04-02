using System.Security.Cryptography;

namespace FinanceManager.Common.Services;

public interface IPasswordHasher
{
    string HashPassword(string password);

    bool CheckPassword(string loginPassword, string storedPassword);
}

public class PasswordHasher(IPasswordUtilities passwordUtilities) : IPasswordHasher
{ 
    public string HashPassword(string password)
    {
        var rng = RandomNumberGenerator.Create();
        var salt = passwordUtilities.CreateSalt();

        rng.GetBytes(salt);

        var passwordKey = passwordUtilities.CreatePasswordKey(password, salt);

        var hashedPassword = passwordUtilities.HashPassword(passwordKey, salt);
        
        return passwordUtilities.HashedPasswordToString(hashedPassword);
    }

    public bool CheckPassword(string loginPassword, string storedPassword)
    {
        var bytes = passwordUtilities.PasswordToBytes(storedPassword);

        var salt = passwordUtilities.ExtractSalt(bytes);

        var passwordKey = passwordUtilities.CreatePasswordKey(loginPassword, salt);

        var compared = passwordUtilities.ComparePassword(passwordKey, salt, bytes);

        return compared;
    }
}