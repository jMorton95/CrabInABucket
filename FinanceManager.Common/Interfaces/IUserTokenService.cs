using System.Security.Claims;
using FinanceManager.Common.DataEntities;
using FinanceManager.Common.Responses;
using FinanceManager.Common.Utilities;

namespace FinanceManager.Common.Interfaces;

public interface IUserTokenService
{
    Task<List<Claim>> GetUserClaims(User user);
    TokenWithExpiryResponse CreateTokenWithClaims(IEnumerable<Claim> claims);
    DecodedAccessToken? DecodeAccessToken(string accessToken);
}