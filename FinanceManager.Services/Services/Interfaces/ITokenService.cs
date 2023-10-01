using System.Security.Claims;
using FinanceManager.Core.Responses;
using FinanceManager.Common.Models;

namespace FinanceManager.Services.Services.Interfaces;

public interface ITokenService
{
    Task<List<Claim>> GetUserClaims(User user);
    TokenWithExpiry CreateTokenWithClaims(IEnumerable<Claim> claims);
}