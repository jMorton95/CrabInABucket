namespace FinanceManager.Data.Read;

public interface IGetEntityByIdAsync<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id);
}