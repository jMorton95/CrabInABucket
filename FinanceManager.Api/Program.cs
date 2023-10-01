using FinanceManager.Application.AppConstants;
using FinanceManager.Core.ConfigurationSettings;
using FinanceManager.Application.DependencyInjection;
using FinanceManager.Api.Endpoints;
using FinanceManager.Application.OpenApi;
using FinanceManager.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(SettingsConstants.JwtSection));
builder.Services.Configure<SwaggerSettings>(builder.Configuration.GetSection(SettingsConstants.SwaggerSection));

builder.AddAuth();

builder.ConfigureSwaggerGeneration();

builder.Services.AddPostgres<DataContext>(builder.Configuration.GetConnectionString(SettingsConstants.PostgresConnection) ?? "");

builder.Services
    .AddHttpContextAccessor()
    .AddEndpointValidators()
    .AddQueries()
    .AddProcesses()
    .AddApplicationServices()
    .AddWorkers();

var app = builder.Build();


app.UseConfiguredSwagger();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapApiEndpoints();

app.Run();