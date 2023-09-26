using CrabInABucket.Core.Services.Dtos;
using CrabInABucket.Data.Models;

namespace CrabInABucket.Core.Services.Interfaces;

public interface ITokenService
{
    Task<TokenDto> CreateToken(User user);
}