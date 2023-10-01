using CrabInABucket.Api.Endpoints;
using CrabInABucket.Application.AppConstants;
using CrabInABucket.Application.ConfigurationSettings;
using CrabInABucket.Application.DependencyInjection;
using CrabInABucket.Application.OpenApi;
using CrabInABucket.Data;

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

app.MapAuthEndpoints();
app.MapUserEndpoints();

app.Run();