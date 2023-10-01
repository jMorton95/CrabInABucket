using CrabInABucket.Api.Endpoints;
using CrabInABucket.Application.AppConstants;
using CrabInABucket.Application.ConfigurationSettings;
using CrabInABucket.Application.DependencyInjection;
using CrabInABucket.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString(SettingsConstants.PostgresConnection) ?? "";

builder.Services.AddPostgres<DataContext>(connectionString);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(SettingsConstants.JwtSection));

builder.AddAuth();

builder.Services.AddValidators();
builder.Services.AddProcesses();
builder.Services.AddApplicationServices();
builder.Services.AddWorkers();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapUserEndpoints();

app.Run();