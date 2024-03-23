namespace FinanceManager.Common.Responses;

public record ResultResponse<T>(bool Success, string Message, T? Result = default);
