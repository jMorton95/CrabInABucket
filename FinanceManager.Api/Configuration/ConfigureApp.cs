using FinanceManager.Api.Middleware.UserContext;
using FinanceManager.Common.Constants;
using FinanceManager.Common.Services;
using FinanceManager.Common.Settings;
using FinanceManager.Data.Seeding;
using Microsoft.Extensions.Options;
using Serilog;

namespace FinanceManager.Api.Configuration;

public static class ConfigureApp
{
    public static async Task Configure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        await app.EnsureDatabaseCreated();
        
        app.UseConfiguredSwagger();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<CurrentUserMiddleware>();

        app.MapEndpoints();
    }
    
    private static void UseConfiguredSwagger(this WebApplication app)
    {
        var swaggerSettings = app.Configuration.GetSection(SettingsConstants.SwaggerSection).Get<SwaggerSettings>();
        
        if (!app.Environment.IsDevelopment())
        {
            return;
        }
        
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{swaggerSettings!.Version}/swagger.json", $"{swaggerSettings.Title} {swaggerSettings.Version}"));
    }

    private static async Task EnsureDatabaseCreated(this WebApplication app)
    {
        string[] developmentEnvironments = ["Development", "Staging", "Test"];

        using var scope = app.Services.CreateScope();
      
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        var passwordHasher = app.Services.GetRequiredService<IPasswordHasher>();

        await dbContext.Database.MigrateAsync();
        
        if (developmentEnvironments.Contains(app.Environment.EnvironmentName))
        {
            await dbContext.InsertTestData(passwordHasher);
        }

        if (app.Environment.IsProduction())
        {
            var settings = app.Services.GetRequiredService<IOptions<SuperAdminSettings>>();
            
            await dbContext.InsertProductionData(settings.Value, passwordHasher);
        }
    }
}