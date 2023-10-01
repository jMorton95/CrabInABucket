using System.Security.Cryptography;
using System.Text;
using CrabInABucket.Application.AppConstants;
using CrabInABucket.Application.ConfigurationSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CrabInABucket.Application.DependencyInjection;

/// <summary>
/// Configures JWT Authentication & Authorization Policies
/// </summary>
public static class AuthRegistration
{
    public static void AddAuth(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection(SettingsConstants.JwtSection).Get<JwtSettings>();
        
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(RoleConstants.AdminRole, policy => policy.RequireClaim(RoleConstants.AdminRole));
        });
    }
}