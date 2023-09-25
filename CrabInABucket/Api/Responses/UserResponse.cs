using CrabInABucket.Models;

namespace CrabInABucket.Api.Responses;

public record UserResponse(string Username, IEnumerable<UserRole> Roles, IEnumerable<Account> Accounts);