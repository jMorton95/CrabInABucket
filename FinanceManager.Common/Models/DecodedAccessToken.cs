namespace FinanceManager.Common.Models;

public record DecodedAccessToken(Guid UserId, Guid Jti, DateTime ExpiryDate, string Audience, List<string>? Roles);