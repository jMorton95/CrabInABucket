using CrabInABucket.Api.Endpoints;
using CrabInABucket.Configurators;
using CrabInABucket.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection") ?? "";

builder.Services.AddPostgres<DataContext>(connectionString);
builder.AddAuth();
builder.Services.AddValidators();
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