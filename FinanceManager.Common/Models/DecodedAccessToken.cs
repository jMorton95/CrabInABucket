namespace FinanceManager.Common.Utilities;

public record DecodedAccessToken(Guid UserId, Guid Jti, DateTime ExpiryDate, string Audience, List<string>? Roles);