using FinanceManager.Core.DataEntities;

namespace FinanceManager.Data.Write;

public interface IWrite<in TEntity> where TEntity : BaseModel
{
    Task<int> CreateAsync(TEntity entity);
    
    Task<int> EditAsync(TEntity entity);
}