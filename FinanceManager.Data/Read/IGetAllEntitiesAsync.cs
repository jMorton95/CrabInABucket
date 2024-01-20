using FinanceManager.Core.DataEntities;

namespace FinanceManager.Data.Read;

public interface IGetAllEntitiesAsync<TEntity> where TEntity : BaseModel
{
    Task<IEnumerable<TEntity>> GetAllAsync();
}