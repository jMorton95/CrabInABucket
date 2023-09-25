using CrabInABucket.Api.Mappers;
using CrabInABucket.Api.Requests;
using CrabInABucket.Api.Responses;
using CrabInABucket.Api.Validation;
using CrabInABucket.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/user/getAll", async Task<Results<Ok<IEnumerable<UserResponse>>, NoContent>> ([FromServices] CrabDbContext db) =>
        {
            var users = await db.User.Include(x => x.Roles).Include(x => x.Accounts).ToListAsync();

            var res = users.Select(x => x.ToUserResponse());
            
            return users.Count > 0 ? TypedResults.Ok(res) : TypedResults.NoContent();
        });
        
        app.MapGet("/api/user/get", async Task<Results<Ok<UserResponse>, ValidationProblem, NoContent>>
            ([FromQuery] GetUserRequest req, [FromServices] CrabDbContext db, IValidator<GetUserRequest> validator) =>
        {
            var validationResult = await validator.ValidateAsync(req);
            
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }
            
            var user = await db.User.Include(x => x.Roles).Include(x => x.Accounts).FirstOrDefaultAsync(x => x.Username == req.Username);
            
            if (user == null)
            {
                return TypedResults.NoContent();
            }
            
            var res = user.ToUserResponse();
                
            return TypedResults.Ok(res);
        });

        app.MapPost("/api/user/create", async Task<Results<Ok<User>, ValidationProblem>>
            ([FromBody] CreateUserRequest req, [FromServices] IValidator<CreateUserRequest> validator, CrabDbContext db) =>
        {
            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var user = new User { Username = req.Username, Password = req.Password };

            await db.User.AddAsync(user);

            await db.SaveChangesAsync();

            return TypedResults.Ok(user);
        });


    }
}