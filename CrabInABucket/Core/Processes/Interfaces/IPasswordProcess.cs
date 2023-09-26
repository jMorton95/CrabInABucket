using System.Security.Cryptography;

namespace CrabInABucket.Core.Processes.Interfaces;

public interface IPasswordProcess
{
    bool ComparePassword(Rfc2898DeriveBytes passwordKey, byte[] salt, byte[] bytes);

    byte[] ExtractSalt(byte[] passwordBytes);

    byte[] PasswordToBytes(string storedPassword);

    Rfc2898DeriveBytes CreatePasswordKey(string loginPassword, byte[] salt);
}