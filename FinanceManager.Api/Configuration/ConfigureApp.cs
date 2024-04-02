using FinanceManager.Api.Features;
using FinanceManager.Api.Middleware.UserContext;
using FinanceManager.Common.Constants;
using FinanceManager.Common.Settings;
using Serilog;

namespace FinanceManager.Api.Configuration;

public static class ConfigureApp
{
    public static void Configure(this WebApplication app)
    {
        app.UseSerilogRequestLogging();
        
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
}