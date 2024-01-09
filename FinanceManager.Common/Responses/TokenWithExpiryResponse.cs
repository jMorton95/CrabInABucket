namespace FinanceManager.Core.Responses;
public record TokenWithExpiryResponse(string Token, long ExpiryDate);