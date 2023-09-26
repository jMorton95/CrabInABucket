using CrabInABucket.Data.Models;

namespace CrabInABucket.Core.Services.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(User user);
}