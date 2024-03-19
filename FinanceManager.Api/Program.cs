using FinanceManager.Core.AppConstants;
using FinanceManager.Core.ConfigurationSettings;
using FinanceManager.Application.DependencyInjection;
using FinanceManager.Api.Endpoints;
using FinanceManager.Application.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(SettingsConstants.JwtSection));
builder.Services.Configure<SwaggerSettings>(builder.Configuration.GetSection(SettingsConstants.SwaggerSection));

builder.AddAuth();

builder.ConfigureSwaggerGeneration();

builder.AddPostgresConnection();

builder.Services
    .AddHttpContextAccessor()
    .AddRequestValidators()
    .AddDataAccessServices()
    .AddAppServices()
    .AddServiceHandlers();

var app = builder.Build();


app.UseConfiguredSwagger();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCustomMiddleware();

app.MapApiEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapDevelopmentEndpoints();
}

app.Run();