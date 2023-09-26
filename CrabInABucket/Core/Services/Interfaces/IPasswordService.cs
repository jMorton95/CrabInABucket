namespace CrabInABucket.Core.Services.Interfaces;

public interface IPasswordService
{
    string HashPassword(string password);

    bool CheckPassword(string loginPassword, string storedPassword);
}