using System.Security.Claims;
using FinanceManager.Core.Responses;
using FinanceManager.Core.Models;
using Microsoft.Extensions.Primitives;

namespace FinanceManager.Services.Services.Interfaces;

public interface ITokenService
{
    Task<List<Claim>> GetUserClaims(User user);
    TokenWithExpiry CreateTokenWithClaims(IEnumerable<Claim> claims);

    DecodedAccessToken DecodeAccessToken(string accessToken);
}