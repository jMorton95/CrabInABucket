using System.Security.Cryptography;
using CrabInABucket.Core.Processes.Interfaces;
using CrabInABucket.Core.Services.Interfaces;

namespace CrabInABucket.Core.Processes;

public class PasswordProcess : IPasswordProcess
{
    public byte[] PasswordToBytes(string storedPassword)
    {
        var bytes = new byte[64]; 
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(storedPassword.Substring(i * 2, 2), 16);
        }

        return bytes;
    }

    public byte[] ExtractSalt(byte[] passwordBytes)
    {
        var salt = new byte[32];
        Array.Copy(passwordBytes, 0, salt, 0, salt.Length);

        return salt;
    }

    public Rfc2898DeriveBytes CreatePasswordKey(string loginPassword, byte[] salt)
    {
        return new Rfc2898DeriveBytes(loginPassword, salt, 100000, HashAlgorithmName.SHA256);
    }

    public bool ComparePassword(Rfc2898DeriveBytes passwordKey, byte[] salt, byte[] bytes)
    {
        var hashBytes = passwordKey.GetBytes(32);
        
        for (var i = 0; i < hashBytes.Length; i++)
        {
            if (hashBytes[i] != bytes[i + salt.Length])
            {
                return false; 
            }
        }
        
        return true; 
    }
}