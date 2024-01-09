namespace FinanceManager.Core.Requests;

public record CreateUserRequest(string Username, string Password, string PasswordConfirmation);
