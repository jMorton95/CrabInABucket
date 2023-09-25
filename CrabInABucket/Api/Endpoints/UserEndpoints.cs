using CrabInABucket.Api.Requests;
using CrabInABucket.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/user/getAll", async ([FromServices] CrabDbContext db) => await db.User.ToListAsync());
        
        app.MapGet("/api/user/get", async ([FromQuery] string username, [FromServices] CrabDbContext db) =>
            {
                var user = await db.User.FirstOrDefaultAsync(x => x.Username == username);

                return user != null ? Results.Ok(user) : Results.NoContent();
            })
            .Produces<User>()
            .Produces(StatusCodes.Status204NoContent);

        app.MapPost("/api/user/create", async ([FromBody] CreateUserRequest req, [FromServices] IValidator<CreateUserRequest> validator, CrabDbContext db) =>
            {
                var validationResult = await validator.ValidateAsync(req);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var user = new User { Username = req.Username, Password = req.Password };

                await db.User.AddAsync(user);

                await db.SaveChangesAsync();

                return Results.Ok(user);
            })
            .Produces<CreateUserRequest>(StatusCodes.Status201Created)
            .Produces<IDictionary<string, string>>(StatusCodes.Status400BadRequest);


    }
}