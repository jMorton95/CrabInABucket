namespace FinanceManager.Common.Responses;

public record LoginResponse(TokenWithExpiryResponse AccessToken, UserResponses User);