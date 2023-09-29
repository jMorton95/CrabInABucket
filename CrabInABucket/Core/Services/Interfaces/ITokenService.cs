using System.Security.Claims;
using CrabInABucket.Core.Services.Dtos;
using CrabInABucket.Data.Models;

namespace CrabInABucket.Core.Services.Interfaces;

public interface ITokenService
{
    Task<List<Claim>> GetUserClaims(User user);
    TokenDto CreateToken(List<Claim> claims);
}