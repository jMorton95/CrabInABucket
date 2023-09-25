namespace CrabInABucket.Api.Requests;

public record CreateUserRequest(string Username, string Password, string PasswordConfirmation);
