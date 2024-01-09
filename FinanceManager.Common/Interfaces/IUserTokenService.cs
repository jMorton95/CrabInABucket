using System.Security.Claims;
using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Responses;
using FinanceManager.Core.Utilities;

namespace FinanceManager.Core.Interfaces;

public interface IUserTokenService
{
    Task<List<Claim>> GetUserClaims(User user);
    TokenWithExpiryResponse CreateTokenWithClaims(IEnumerable<Claim> claims);
    DecodedAccessToken? DecodeAccessToken(string accessToken);
}