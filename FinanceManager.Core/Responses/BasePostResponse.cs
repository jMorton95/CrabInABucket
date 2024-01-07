namespace FinanceManager.Core.Responses;

public record BasePostResponse(bool Success, string Message = "");