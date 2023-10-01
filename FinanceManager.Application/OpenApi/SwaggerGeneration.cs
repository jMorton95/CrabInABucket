using FinanceManager.Application.AppConstants;
using FinanceManager.Application.ConfigurationSettings;
using Microsoft.AspNetCore.Builder;

namespace FinanceManager.Application.OpenApi;

public static class SwaggerGeneration
{
    public static void ConfigureSwaggerGeneration(this WebApplicationBuilder builder)
    {
        var swaggerSettings = builder.Configuration.GetSection(SettingsConstants.SwaggerSection).Get<SwaggerSettings>();

        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen(options =>
        {
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

    public static void UseConfiguredSwagger(this WebApplication app)
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