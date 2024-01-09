namespace FinanceManager.Core.Responses;

public record LoginResponse(TokenWithExpiryResponse AccessToken, UserResponse User);