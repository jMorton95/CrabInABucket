using System.Security.Cryptography;
using System.Text;
using CrabInABucket.Core.Processes.Interfaces;
using CrabInABucket.Core.Services.Interfaces;

namespace CrabInABucket.Core.Processes;

public class PasswordProcess : IPasswordProcess
{
    public byte[] CreateSalt() => new byte[32];

    public byte[] HashPassword(Rfc2898DeriveBytes passwordKey, byte[] salt)
    {
        var passwordBytes = passwordKey.GetBytes(32);
        var hashedPassword = new byte[salt.Length + passwordBytes.Length];
        
        Array.Copy(salt, 0, hashedPassword, 0, salt.Length);
        
        Array.Copy(passwordBytes, 0, hashedPassword, salt.Length, passwordBytes.Length);

        return hashedPassword;
    }

    public string HashedPasswordToString(byte[] bytePassword)
    {
        var builder = new StringBuilder(bytePassword.Length * 2);
        
        foreach (var b in bytePassword)
        {
            builder.Append(b.ToString("x2"));
        }
        
        return builder.ToString();
    }
    
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
        var salt = CreateSalt();
        Array.Copy(passwordBytes, 0, salt, 0, salt.Length);

        return salt;
    }

    public Rfc2898DeriveBytes CreatePasswordKey(string loginPassword, byte[] salt)
    {
        return new Rfc2898DeriveBytes(loginPassword, salt, 100000, HashAlgorithmName.SHA256);
    }

    public bool ComparePassword(Rfc2898DeriveBytes passwordKey, byte[] salt, byte[] passwordBytes)
    {
        var hashBytes = passwordKey.GetBytes(32);
        
        for (var i = 0; i < hashBytes.Length; i++)
        {
            if (hashBytes[i] != passwordBytes[i + salt.Length])
            {
                return false; 
            }
        }

        return true;
    }
}