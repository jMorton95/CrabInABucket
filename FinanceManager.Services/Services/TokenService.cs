using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinanceManager.Core.Responses;
using FinanceManager.Data;
using FinanceManager.Core.ConfigurationSettings;
using FinanceManager.Core.Models;
using FinanceManager.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace FinanceManager.Services.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwt;
    private readonly DataContext _db;

    public TokenService(IOptions<JwtSettings> jwt, DataContext db)
    {
        _jwt = jwt.Value;
        _db = db;
    }

    public async Task<List<Claim>> GetUserClaims(User user)
    {
        var userRoles = new List<Role>();

        if (user.Roles != null && user.Roles.Any())
        {
            var roleIds = user.Roles.Select(r => r.Role!.Id).ToList();
            userRoles = await _db.Role.Where(role => roleIds.Contains(role.Id)).ToListAsync();
        }
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
        
        return claims;
    }
    
    public TokenWithExpiry CreateTokenWithClaims(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_jwt.ExpireDays));

        var token = new JwtSecurityToken(
            _jwt.Issuer,
            _jwt.Audience,
            claims,
            expires: expires,
            signingCredentials: credentials
        );
        
        var expiresUnixTimestamp = ((DateTimeOffset)expires).ToUnixTimeSeconds();

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
        return new TokenWithExpiry(jwt, expiresUnixTimestamp);
    }

    public DecodedAccessToken DecodeAccessToken(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;

        var userId = Guid.Parse(jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value);
        var jti = Guid.Parse(jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value);
        var expiryDate = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value))
            .DateTime;
        var roles = jsonToken.Claims.Where(claim => claim.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
        var audience = jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Aud).Value;

        return new DecodedAccessToken(userId, jti, expiryDate, audience, roles);
    }

}