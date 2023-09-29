using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CrabInABucket.Core.Services.Dtos;
using CrabInABucket.Core.Services.Interfaces;
using CrabInABucket.Data;
using CrabInABucket.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CrabInABucket.Core.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly DataContext _db;

    public TokenService(IConfiguration configuration, DataContext db)
    {
        _configuration = configuration;
        _db = db;
    }

    public async Task<List<Claim>> GetUserClaims(User user)
    {
        var userRoles = new List<Role>();

        if (user.Roles != null && user.Roles.Any())
        {
            var roleIds = user.Roles.Select(r => r.Role.Id).ToList();
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
    
    public TokenDto CreateTokenWithClaims(List<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: credentials
        );
        
        var expiresUnixTimestamp = ((DateTimeOffset)expires).ToUnixTimeSeconds();

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
        return new TokenDto(jwt, expiresUnixTimestamp);
    }
}