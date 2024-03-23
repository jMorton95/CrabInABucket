namespace FinanceManager.Common.Requests;

public record CreateUserRequest(string Username, string Password, string PasswordConfirmation);
