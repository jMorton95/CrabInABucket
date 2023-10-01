namespace FinanceManager.Core.Responses;

public record LoginResponse(TokenWithExpiry AccessToken, UserResponse User);