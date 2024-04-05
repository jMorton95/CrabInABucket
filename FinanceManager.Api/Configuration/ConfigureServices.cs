using System.Text;
using FinanceManager.Common.Constants;
using FinanceManager.Common.Mappers;
using FinanceManager.Common.Services;
using FinanceManager.Common.Settings;
using FinanceManager.Data.Read.Accounts;
using FinanceManager.Data.Read.Friends;
using FinanceManager.Data.Read.Friendships;
using FinanceManager.Data.Read.Users;
using FinanceManager.Data.Write.Accounts;
using FinanceManager.Data.Write.Friendships;
using FinanceManager.Data.Write.Transactions;
using FinanceManager.Data.Write.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace FinanceManager.Api.Configuration;

public static class ConfigureServices
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.AddSerilog();
        builder.AddSwaggerGeneration();
        
        builder.ConfigureOptions();
        builder.AddDatabase();

        builder.Services.AddValidatorsFromAssembly(typeof(ConfigureServices).Assembly);
        
        builder.Services.RegisterTransientDependencies();
        builder.Services.RegisterScopedDependencies();
        
        builder.AddJwtAuthentication();
        
        builder.Services.AddHttpContextAccessor();
    }
    
    private static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
    }

    private static void ConfigureOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(SettingsConstants.JwtSection));
        builder.Services.Configure<SwaggerSettings>(builder.Configuration.GetSection(SettingsConstants.SwaggerSection));
    }
    
    private static void RegisterTransientDependencies(this IServiceCollection services)
    {
        services.AddTransient<IPasswordUtilities, PasswordUtilities>();
        services.AddTransient<ITransactionMapper, TransactionMapper>();        
        services.AddTransient<IUserTokenService, UserTokenService>();
        services.AddTransient<IPasswordHasher, PasswordHasher>();
    }

    private static void RegisterScopedDependencies(this IServiceCollection services)
    {
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IReadUsers, ReadUsers>();
        services.AddScoped<IWriteUsers, WriteUsers>();
        
        services.AddScoped<IReadAccounts, ReadAccounts>();
        services.AddScoped<IWriteAccounts, WriteAccounts>();

        services.AddScoped<IWriteTransaction, WriteTransaction>();

        services.AddScoped<IWriteRecurringTransaction, WriteRecurringTransaction>();

        services.AddScoped<IReadFriendships, ReadFriendships>();
        services.AddScoped<IWriteFriendships, WriteFriendships>();

        services.AddScoped<IReadUserFriends, ReadUserFriends>();
    }
    
    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(SettingsConstants.PostgresConnection);
        builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));
    }
    
    private static void AddJwtAuthentication(this WebApplicationBuilder builder)
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
                ValidIssuer = jwtSettings?.Issuer,
                ValidAudience = jwtSettings?.Audience,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key ?? ""))
            };
        });

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(PolicyConstants.AuthenticatedUser, policy => policy.RequireAuthenticatedUser())
            .AddPolicy(PolicyConstants.AdminRole, policy => policy.RequireClaim(PolicyConstants.AdminRole));
    }
    
    private static void AddSwaggerGeneration(this WebApplicationBuilder builder)
    {
        var swaggerSettings = builder.Configuration.GetSection(SettingsConstants.SwaggerSection).Get<SwaggerSettings>();

        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type => type.FullName?.Replace('+', '.'));
            
            options.SwaggerDoc(swaggerSettings!.Version, new OpenApiInfo { Title = swaggerSettings.Title, Version = swaggerSettings.Version });

            options.AddSecurityDefinition(swaggerSettings.Scheme, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = swaggerSettings.Description,
                Name = swaggerSettings.Name,
                Type = SecuritySchemeType.Http,
                Scheme = swaggerSettings.Scheme,
                BearerFormat = swaggerSettings.BearerFormat
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = swaggerSettings.Scheme }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
    
   
}