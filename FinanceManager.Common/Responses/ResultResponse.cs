namespace FinanceManager.Core.Responses;

public record ResultResponse<T>(bool Success, string Message, T? Result = default);
