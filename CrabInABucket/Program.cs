using CrabInABucket.Api.Endpoints;
using CrabInABucket.Configurators;
using CrabInABucket.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection") ?? "";

builder.Services.AddDbContext<CrabDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();


builder.Services.AddValidators();
builder.Services.AddServices();
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