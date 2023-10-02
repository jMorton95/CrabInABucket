using System.Text;
using FinanceManager.Application.AppConstants;
using FinanceManager.Core.AppConstants;
using FinanceManager.Core.ConfigurationSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FinanceManager.Application.DependencyInjection;

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