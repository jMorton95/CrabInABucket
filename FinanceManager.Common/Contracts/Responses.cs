namespace FinanceManager.Common.Contracts;

public interface IPostResponse
{
    bool Success { get; init; }
    string Message { get; init; }
}