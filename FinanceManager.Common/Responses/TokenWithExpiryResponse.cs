namespace FinanceManager.Common.Responses;
public record TokenWithExpiryResponse(string Token, long ExpiryDate);