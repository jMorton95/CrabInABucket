using CrabInABucket.Api;
using CrabInABucket.Api.Endpoints;
using CrabInABucket.Api.Requests;
using CrabInABucket.Api.Validation;
using CrabInABucket.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection") ?? "";

builder.Services.AddDbContext<CrabDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
builder.Services.AddScoped<IValidator<GetUserRequest>, GetUserValidator>();



var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();

app.Run();