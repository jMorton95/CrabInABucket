using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinanceManager.Core.Responses;
using FinanceManager.Data;
using FinanceManager.Core.ConfigurationSettings;
using FinanceManager.Core.DataEntities;
using FinanceManager.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FinanceManager.Services.Services;

public interface IUserTokenService
{
    Task<List<Claim>> GetUserClaims(User user);
    TokenWithExpiryResponse CreateTokenWithClaims(IEnumerable<Claim> claims);
    DecodedAccessToken? DecodeAccessToken(string accessToken);
}

public class UserTokenService(IOptions<JwtSettings> jwtOptions, DataContext db) : IUserTokenService
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;
    public async Task<List<Claim>> GetUserClaims(User user)
    {
        var userRoles = new List<Role>();

        if (user.Roles.Any())
        {
            var roleIds = user.Roles.Select(r => r.Role!.Id).ToList();
            userRoles = await db.Role.Where(role => roleIds.Contains(role.Id)).ToListAsync();
        }
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        
        return claims;
    }
    
    public TokenWithExpiryResponse CreateTokenWithClaims(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_jwtSettings.ExpireDays));

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: expires,
            signingCredentials: credentials
        );
        
        var expiresUnixTimestamp = ((DateTimeOffset)expires).ToUnixTimeSeconds();

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
        return new TokenWithExpiryResponse(jwt, expiresUnixTimestamp);
    }

    public DecodedAccessToken? DecodeAccessToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();

        if (handler.ReadToken(accessToken) is not JwtSecurityToken jsonToken)
        {
            return null;
        }

        var userId = Guid.Parse(jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value);
        var jti = Guid.Parse(jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value);
        var expiryDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value))
            .DateTime;
        var roles = jsonToken.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
        var audience = jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Aud).Value;

        return new DecodedAccessToken(userId, jti, expiryDate, audience, roles);
    }

}