using FinanceManager.Core.Mappers;
using FinanceManager.Core.Requests;
using FinanceManager.Core.Responses;
using FinanceManager.Data;
using FinanceManager.Data.Read.Users;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Api.Endpoints.Private;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var usersGroup = app.MapGroup("/api/user/").WithTags("User");//.RequireAuthorization();
        
        usersGroup.MapGet("/getAll", async Task<Results<Ok<List<UserResponse>>, NoContent>>
            ([FromServices] IReadUsers query) =>
        {
            var users = await query.GetAllAsync();
        
            var res = users.Select(x => x.ToUserResponse()).ToList();
            
            return res.Count > 0 ? TypedResults.Ok(res) : TypedResults.NoContent();
        })
        .WithName("GetAll");
       
        usersGroup.MapGet("/get", async Task<Results<Ok<UserResponse>, ValidationProblem, NoContent>>
        (
            string username,
            IValidator<GetUserRequest> validator,
            [FromServices] IReadUsers query
        ) 
            =>
        {
            var validationResult = await validator.ValidateAsync(new GetUserRequest(username));
            
            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }

            var user = await query.GetUserByEmailAsync(username);
            
            if (user == null)
            {
                return TypedResults.NoContent();
            }
            
            var res = user.ToUserResponse();
                
            return TypedResults.Ok(res);
        })
        .WithName("GetByUsername");
    }
}