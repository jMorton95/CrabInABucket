namespace FinanceManager.Data.Read;

public interface IGetEntityByIdAsync<TEntity>
{
    Task<TEntity?> GetByIdAsync(Guid id);
}

public interface IGetOwnedEntityByIdAsync<TEntity>
{
    Task<TEntity?> GetOwnedEntityByIdAsync(Guid id, Guid userId);
}