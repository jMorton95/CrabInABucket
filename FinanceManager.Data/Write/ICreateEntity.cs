using FinanceManager.Common.DataEntities;

namespace FinanceManager.Data.Write;

public interface ICreateEntity<in TEntity> where TEntity : Entity
{
    Task<bool> CreateAsync(TEntity entity);
}
