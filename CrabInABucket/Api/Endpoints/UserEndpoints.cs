using CrabInABucket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrabInABucket.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/user/getAll", async ([FromServices] CrabDbContext db) => await db.User.ToListAsync());
    }
}