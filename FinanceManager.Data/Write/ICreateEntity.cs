using FinanceManager.Core.DataEntities;

namespace FinanceManager.Data.Write;

public interface ICreateEntity<in TEntity> where TEntity : BaseModel
{
    Task<bool> CreateAsync(TEntity entity);
}
