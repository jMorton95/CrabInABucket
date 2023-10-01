using CrabInABucket.Data.Models;
namespace CrabInABucket.Api.Responses;

public record UserResponse(string Username, List<string> Roles, IEnumerable<string> Accounts);