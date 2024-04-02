using FinanceManager.Common.Entities;

namespace FinanceManager.Data.Write;

public interface ICreateEntity<in TEntity> where TEntity : Entity
{
    Task<bool> CreateAsync(TEntity entity);
}
